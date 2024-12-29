using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Servicelayer.Models;

namespace Servicelayer.Data;

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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentEvent> DepartmentEvents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventName> EventNames { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialComment> MaterialComments { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<WorkingCalendar> WorkingCalendars { get; set; }

    public virtual DbSet<Сandidate> Сandidates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI; Database=Profiki; Trusted_Connection=True; Trust Server Certificate = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbsenceEvent>(entity =>
        {
            entity.ToTable("AbsenceEvent");

            entity.Property(e => e.AbsenceType).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.HasOne(d => d.Employee).WithMany(p => p.AbsenceEventEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AbsenceEvent_Employee");

            entity.HasOne(d => d.ReplasementEmployee).WithMany(p => p.AbsenceEventReplasementEmployees)
                .HasForeignKey(d => d.ReplasementEmployeeId)
                .HasConstraintName("FK_AbsenceEvent_Employee1");
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

            entity.HasOne(d => d.HeadDepartmentNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.HeadDepartment)
                .HasConstraintName("FK_Department_Employee");
        });

        modelBuilder.Entity<DepartmentEvent>(entity =>
        {
            entity.HasKey(e => new { e.DapartmentId, e.EventId });

            entity.ToTable("DepartmentEvent");

            entity.Property(e => e.DapartmentId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Event).WithMany(p => p.DepartmentEvents)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentEvent_Event");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Cabinet).HasMaxLength(50);
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Surname).HasMaxLength(100);
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.DirectManagerNavigation).WithMany(p => p.InverseDirectManagerNavigation)
                .HasForeignKey(d => d.DirectManager)
                .HasConstraintName("FK_Employee_Employee");

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
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);

            entity.HasOne(d => d.EventName).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_EventName");

            entity.HasOne(d => d.ResponsibleEmployee).WithMany(p => p.EventsNavigation)
                .HasForeignKey(d => d.ResponsibleEmployeeId)
                .HasConstraintName("FK_Event_Employee");
        });

        modelBuilder.Entity<EventName>(entity =>
        {
            entity.ToTable("EventName");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Material");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(15);

            entity.HasMany(d => d.Events).WithMany(p => p.Materioals)
                .UsingEntity<Dictionary<string, object>>(
                    "EventMaterial",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventMaterial_Event"),
                    l => l.HasOne<Material>().WithMany()
                        .HasForeignKey("MaterioalId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventMaterial_Material"),
                    j =>
                    {
                        j.HasKey("MaterioalId", "EventId");
                        j.ToTable("EventMaterial");
                    });
        });

        modelBuilder.Entity<MaterialComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("MaterialComment");

            entity.Property(e => e.Comment).HasMaxLength(300);

            entity.HasOne(d => d.Employee).WithMany(p => p.MaterialComments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaterialComment_Employee");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialComments)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaterialComment_Material");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<WorkingCalendar>(entity =>
        {
            entity.HasKey(e => e.WorkingCalendarId).HasName("WorkingCalendar_pk");

            entity.ToTable("WorkingCalendar", tb => tb.HasComment("Список дней исключений в производственном календаре"));

            entity.Property(e => e.WorkingCalendarId)
                .ValueGeneratedNever()
                .HasComment("Идентификатор строки");
            entity.Property(e => e.ExceptionDate).HasComment("День-исключение");
            entity.Property(e => e.IsWorkingDay).HasComment("0 - будний день, но законодательно принят выходным");
        });

        modelBuilder.Entity<Сandidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId);

            entity.ToTable("Сandidate");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Resume).HasMaxLength(300);
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasOne(d => d.DesiredPositionNavigation).WithMany(p => p.Сandidates)
                .HasForeignKey(d => d.DesiredPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Сandidate_Position");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
