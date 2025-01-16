using DataLayer.DataContexts;
using DataLayer.DTOs;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController(AppDbContext context) : ControllerBase
    {
        [HttpPatch("{id}")]
        public async Task<IActionResult> DismissEmployeeAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            var absences = await context.AbsenceEvents
                .Where(e => e.EmployeeId == id)
                .ToListAsync();

            context.AbsenceEvents.RemoveRange(absences);

            employee.DismissalDate = DateOnly.FromDateTime(DateTime.Now);
            context.Employees.Update(employee);
            await context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAsync(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
                return BadRequest();

            context.Entry(employee).State = EntityState.Modified;
            await context.SaveChangesAsync();
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

        [HttpPost]
        public async Task<IActionResult> PostEmployeeAsync(Employee employee)
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
            return Ok(employee);
        }
    }
}