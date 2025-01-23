using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WorkingCalendarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkingCalendarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/WorkingCalendars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkingCalendar>>> GetWorkingCalendars()
            => await _context.WorkingCalendars.ToListAsync();
    }
}
