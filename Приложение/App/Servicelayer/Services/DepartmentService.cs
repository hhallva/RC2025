using Microsoft.EntityFrameworkCore;
using ServiceLayer.DataContexts;
using ServiceLayer.Models;

namespace ServiceLayer.Services
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
