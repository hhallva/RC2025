using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.RSS;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel(RssService rssService, EmployeeService employeeService, EventsService eventsService) : PageModel
    {
        public List<RssItem> RssItems { get; set; }
        public List<Employee?> Employees { get; set; }
        public List<EventDto?> Events { get; set; }

        public async Task OnGetAsync()
        {
            RssItems = await rssService.GetRssItemsAsync();
            Employees = await employeeService.GetAllAsync();
            Events = await eventsService.GetAllAsync();
        }
    }
}
