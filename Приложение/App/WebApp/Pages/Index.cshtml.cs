using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.RSS;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using WebApp.Calendar;

namespace WebApp.Pages
{
    public class IndexModel(RssService rssService, EmployeeService employeeService, EventsService eventsService, WorkingCalendarService workingCalendarService) : PageModel
    {
        public List<RssItem> RssItems { get; set; } = new();
        public List<Employee?> Employees { get; set; } = new();
        public List<EventDto?> Events { get; set; } = new();
        public List<WorkingCalendar> ExeptionDays { get; set; } = new();
        public CalendarViewModel CalendarViewModel { get; set; }

        public string SearchTerm {  get; set; }


        public async Task<IActionResult> OnGetAsync(string? month, string searchTerm = null)
        {
            RssItems = await rssService.GetRssItemsAsync();
            Employees = await employeeService.GetAllAsync();
            Events = await eventsService.GetAllAsync();
            ExeptionDays = await  workingCalendarService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchTerm = searchTerm;
                Employees = Employees
                    .Where(e => 
                        e.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        e.Position.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        e.Birthday.Value.ToString("d MMMM").Contains(searchTerm))
                    .ToList();

                Events = Events
                   .Where(e =>
                       e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       e.Annotation.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       e.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       e.Date.ToShortDateString().Contains(searchTerm))
                   .ToList();

                RssItems = RssItems
                   .Where(r =>
                       r.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       r.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       r.PublicationDate.ToShortDateString().Contains(searchTerm))
                   .ToList();
            }


            if (month == "prev")
                CalendarHelper.AddMonths(-1);
            else if (month == "next")
                CalendarHelper.AddMonths(1);
            else
                CalendarHelper.SetCurrentMonth();

            CalendarViewModel = new(ExeptionDays, Employees, Events);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            EventDto eventDto = (await eventsService.GetAllAsync()).FirstOrDefault(e => e.Id == id);

            StringBuilder dataIcs = new();
            dataIcs.AppendLine("BEGIN:VCALENDAR");
            dataIcs.AppendLine("VERSION:2.0");
            dataIcs.AppendLine($"SUMMARY:{eventDto.Title}");
            dataIcs.AppendLine($"DTSTART:{eventDto.Date}");
            dataIcs.AppendLine($"UID:{eventDto.Id}");
            dataIcs.AppendLine($"DESCRIPTION:{eventDto.Annotation} ");
            dataIcs.AppendLine($"ORGANIZER:{eventDto.Author} ");
            dataIcs.AppendLine("STATUS:CONFIRMED");
            dataIcs.AppendLine("PRIORITY:0");
            dataIcs.AppendLine("END:VEVENT");
            dataIcs.AppendLine("END:VCALENDAR");

            
            var fileName = $"{id}_{DateTime.Now.ToString("yyyy-MM-dd HHmmss")}.ics";
            var downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            var filePath = Path.Combine(downloadPath, fileName);

            System.IO.File.WriteAllText(filePath, dataIcs.ToString());
            return RedirectToPage();
        }
    }
}
