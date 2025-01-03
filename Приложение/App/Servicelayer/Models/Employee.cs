using System;
using System.Collections.Generic;

namespace ServiceLayer.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string DepartmentId { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string WorkPhone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Cabinet { get; set; } = null!;

    public int PositionId { get; set; }

    public int? DirectManager { get; set; }

    public DateOnly? Birthday { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<AbsenceEvent> AbsenceEventEmployees { get; set; } = new List<AbsenceEvent>();

    public virtual ICollection<AbsenceEvent> AbsenceEventReplasementEmployees { get; set; } = new List<AbsenceEvent>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual Employee? DirectManagerNavigation { get; set; }

    public virtual ICollection<Event> EventsNavigation { get; set; } = new List<Event>();

    public virtual ICollection<Employee> InverseDirectManagerNavigation { get; set; } = new List<Employee>();

    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
