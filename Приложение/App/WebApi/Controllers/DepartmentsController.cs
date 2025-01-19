using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DataContexts;
using DataLayer.Models;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController(AppDbContext context) : ControllerBase
    {

        // GET: api/v1/Department/{id}/Employees
        [HttpGet("{id}/Employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetDapartmentEmployeesAsync(string id)
            => await context.Employees
            .Where(e => e.DepartmentId == id)
            .ToListAsync();
    }
}
