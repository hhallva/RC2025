namespace DataLayer.Models;

public partial class Department
{
    public string DepartmentId { get; set; } = null!;

    public string? ParentDepartmentId { get; set; }

    public string? Description { get; set; }

    public int? HeadDepartment { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Employee? HeadDepartmentNavigation { get; set; }

    public virtual ICollection<Department> ChildDepartment { get; set; } = new List<Department>();

    public virtual Department? ParentDepartment { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
