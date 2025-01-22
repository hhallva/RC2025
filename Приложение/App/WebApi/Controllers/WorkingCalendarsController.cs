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

        // GET: api/WorkingCalendars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkingCalendar>>> GetWorkingCalendars()
        {
            return await _context.WorkingCalendars.ToListAsync();
        }

        // GET: api/WorkingCalendars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkingCalendar>> GetWorkingCalendar(long id)
        {
            var workingCalendar = await _context.WorkingCalendars.FindAsync(id);

            if (workingCalendar == null)
            {
                return NotFound();
            }

            return workingCalendar;
        }

        // PUT: api/WorkingCalendars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkingCalendar(long id, WorkingCalendar workingCalendar)
        {
            if (id != workingCalendar.WorkingCalendarId)
            {
                return BadRequest();
            }

            _context.Entry(workingCalendar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkingCalendarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WorkingCalendars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkingCalendar>> PostWorkingCalendar(WorkingCalendar workingCalendar)
        {
            _context.WorkingCalendars.Add(workingCalendar);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WorkingCalendarExists(workingCalendar.WorkingCalendarId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWorkingCalendar", new { id = workingCalendar.WorkingCalendarId }, workingCalendar);
        }

        // DELETE: api/WorkingCalendars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkingCalendar(long id)
        {
            var workingCalendar = await _context.WorkingCalendars.FindAsync(id);
            if (workingCalendar == null)
            {
                return NotFound();
            }

            _context.WorkingCalendars.Remove(workingCalendar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkingCalendarExists(long id)
        {
            return _context.WorkingCalendars.Any(e => e.WorkingCalendarId == id);
        }
    }
}
