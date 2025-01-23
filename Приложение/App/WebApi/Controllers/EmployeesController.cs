using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Employee>> GetAllEmployeesAsync()
        {
            var employee = await context.Employees
                .Include(e => e.Position)
                .ToListAsync();

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeAsync(int id)
        {
            var employee = await context.Employees
                .Include(e => e.Events)
                .Include(e => e.AbsenceEventEmployees)
                .Include(e => e.Position)
                .SingleOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAsync(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
                return BadRequest();

            context.Entry(employee).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(employee);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DismissEmployeeAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            var absences = await context.AbsenceEvents
                .Where(a => a.EmployeeId == id)
                .Where(e => e.StartDate.AddDays(e.DaysCount - 1) > DateTime.Now)
                .ToListAsync();

            context.AbsenceEvents.RemoveRange(absences);

            employee.DismissalDate = DateTime.Now;
            context.Employees.Update(employee);
            await context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployeeAsync(Employee employee)
        {
            employee.Position = context.Positions.Find(employee.PositionId);

            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPost("{id}/AbsenceEvent")]
        public async Task<ActionResult<AbsenceEvent>> PostAbsenseEventAsync(int id, AbsenceEvent absenceEvent)
        {
            absenceEvent.EmployeeId = context.Employees.Find(absenceEvent.EmployeeId).EmployeeId;

            await context.AbsenceEvents.AddAsync(absenceEvent);
            await context.SaveChangesAsync();
            return Ok(absenceEvent);
        }

        [HttpPost("{id}/Event")]
        public async Task<ActionResult<AbsenceEvent>> PostEventAsync(int id, Event newEvent)
        {
            var employee = await context.Employees.FindAsync(id);
            newEvent.Employees.Add(employee);

            await context.Events.AddAsync(newEvent);
            await context.SaveChangesAsync();
            return Ok(newEvent);
        } 
    }
}