using System;
using System.Collections.Generic;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataContexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AbsenceEvent> AbsenceEvents { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<WorkingCalendar> WorkingCalendars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=T1;User ID=hhallva;Password=123890;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbsenceEvent>(entity =>
        {
            entity.ToTable("AbsenceEvent");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.AbsenceEventEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AbsenceEvent_Employee");

            entity.HasOne(d => d.ReplacementEmployee).WithMany(p => p.AbsenceEventReplacementEmployees)
                .HasForeignKey(d => d.ReplacementEmployeeId)
                .HasConstraintName("FK_AbsenceEvent_Employee1");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.ToTable("Candidate");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Resume).HasMaxLength(300);
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasOne(d => d.Position).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Candidate_Position");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Text).HasMaxLength(300);

            entity.HasOne(d => d.Document).WithMany(p => p.Comments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Document");

            entity.HasOne(d => d.Employee).WithMany(p => p.Comments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Employee");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department", tb => tb.HasTrigger("trAddParentDepartment"));

            entity.Property(e => e.DepartmentId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ParentDepartmentId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.HeadDepartment).WithMany(p => p.Departments)
                .HasForeignKey(d => d.HeadDepartmentId)
                .HasConstraintName("FK_Department_Employee");

            entity.HasOne(d => d.ParentDepartment).WithMany(p => p.ChildDepartment)
                .HasForeignKey(d => d.ParentDepartmentId)
                .HasConstraintName("FK_Department_Department");

            entity.HasMany(d => d.Events).WithMany(p => p.Departments)
                .UsingEntity<Dictionary<string, object>>(
                    "DepartmentEvent",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DepartmentEvent_Event"),
                    l => l.HasOne<Department>().WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DepartmentEvent_Department"),
                    j =>
                    {
                        j.HasKey("DepartmentId", "EventId");
                        j.ToTable("DepartmentEvent");
                        j.IndexerProperty<string>("DepartmentId")
                            .HasMaxLength(20)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(15);

            entity.HasOne(d => d.Author).WithMany(p => p.Documents)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Cabinet).HasMaxLength(10);
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Surname).HasMaxLength(100);
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Department");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Position");

            entity.HasMany(d => d.Events).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeEvent",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeEvent_Event"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeEvent_Employee"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "EventId");
                        j.ToTable("EmployeeEvent");
                    });
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Event");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.ResponsibleEmployee).WithMany(p => p.EventsNavigation)
                .HasForeignKey(d => d.ResponsibleEmployeeId)
                .HasConstraintName("FK_Event_Employee");

            entity.HasOne(d => d.Type).WithMany(p => p.Events)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_EventType");

            entity.HasMany(d => d.Documents).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventDocument",
                    r => r.HasOne<Document>().WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventDocument_Document"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventDocument_Event"),
                    j =>
                    {
                        j.HasKey("EventId", "DocumentId");
                        j.ToTable("EventDocument");
                    });
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.ToTable("EventType");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<WorkingCalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WorkingCalendar_pk");

            entity.ToTable("WorkingCalendar", tb => tb.HasComment("Список дней исключений в производственном календаре"));

            entity.Property(e => e.Id).HasComment("Идентификатор строки");
            entity.Property(e => e.ExceptionDate).HasComment("День-исключение");
            entity.Property(e => e.IsWorkingDay).HasComment("0 - будний день, но законодательно принят выходным");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
