using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public class DepartmentService(AppDbContext context)
    {
        public async Task<List<Department>> GetDepartmentsAsync()
           => await context.Departments
            .Include(d => d.ChildDepartment)
            .Include(d => d.Employees)
            .ThenInclude(e => e.Position)
            .ToListAsync();
    }
}
