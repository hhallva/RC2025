using Microsoft.EntityFrameworkCore;
using DataLayer.DataContexts;
using DataLayer.Models;

namespace DataLayer.Services
{
    public class DepartmentService(AppDbContext context)
    {
        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
           => await context.Departments
            .Include(d => d.InverseParentDepartment)
            .Include(d => d.Employees)
            .ThenInclude(e => e.Position)
            .ToListAsync();
    }
}
