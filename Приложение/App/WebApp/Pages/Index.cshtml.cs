using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Calendar;

namespace WebApp.Pages
{
    public class IndexModel(WorkingCalendarService calendarService, NewsService newsService,EventsService eventsService, EmployeeService employeeService) : PageModel
    {
        public List<Employee?> Employees { get; set; } = new();
        public List<EventDto?> Events { get; set; } = new();
        public List<NewsDto?> News { get; set; } = new();
        public List<WorkingCalendar?> ExeptionDays { get; set; } = new();


        public CalendarViewModel CalendarViewModel { get; set; }
        public string? SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync(string? month,string? searthTerm = null)
        {
            Employees = await employeeService.GetAllAsync();
            Events = await eventsService.GetAllAsync();
            News = await newsService.GetAllAsync();


            if(searthTerm != null)
            {
                Employees = Employees
                    .Where(e => e.FullName.Contains(searthTerm, StringComparison.OrdinalIgnoreCase) ||
                                e.Position.Name.Contains(searthTerm, StringComparison.OrdinalIgnoreCase) ||
                                e.Birthday.Value.ToString("d MMMM").Contains(searthTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Events = Events
                 .Where(e => e.Title.Contains(searthTerm, StringComparison.OrdinalIgnoreCase) ||
                             e.Description.Contains(searthTerm, StringComparison.OrdinalIgnoreCase) ||
                             e.Date.ToShortDateString().Contains(searthTerm, StringComparison.OrdinalIgnoreCase))
                 .ToList();

                News = News
                .Where(e => e.Title.Contains(searthTerm, StringComparison.OrdinalIgnoreCase) ||
                            e.Description.Contains(searthTerm, StringComparison.OrdinalIgnoreCase) ||
                            e.PublicationDate.ToShortDateString().Contains(searthTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }

            if (month == "prev") CalendarHelper.AddMonths(-1);
            else if (month == "next") CalendarHelper.AddMonths(1);
            else CalendarHelper.SetCurrentMonth();

            CalendarViewModel = new(ExeptionDays, Employees, Events);
            return Page();
        }
    }
}