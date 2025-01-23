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

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<WorkingCalendar> WorkingCalendars { get; set; }

    public virtual DbSet<Сandidate> Сandidates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
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

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK_MaterialComment");

            entity.ToTable("Comment");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Text).HasMaxLength(300);

            entity.HasOne(d => d.Document).WithMany(p => p.Comments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaterialComment_Material");

            entity.HasOne(d => d.Employee).WithMany(p => p.Comments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaterialComment_Employee");
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

            entity.HasOne(d => d.ParentDepartment).WithMany(p => p.ChildDepartment)
                .HasForeignKey(d => d.ParentDepartmentId)
                .HasConstraintName("FK_Department_Department");

            entity.HasMany(d => d.Events).WithMany(p => p.Dapartments)
                .UsingEntity<Dictionary<string, object>>(
                    "DepartmentEvent",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DepartmentEvent_Event"),
                    l => l.HasOne<Department>().WithMany()
                        .HasForeignKey("DapartmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DepartmentEvent_Department"),
                    j =>
                    {
                        j.HasKey("DapartmentId", "EventId");
                        j.ToTable("DepartmentEvent");
                        j.IndexerProperty<string>("DapartmentId")
                            .HasMaxLength(20)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK_Material");

            entity.ToTable("Document");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(15);

            entity.HasMany(d => d.Events).WithMany(p => p.Documents)
                .UsingEntity<Dictionary<string, object>>(
                    "EventDocument",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventMaterial_Event"),
                    l => l.HasOne<Document>().WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventMaterial_Material"),
                    j =>
                    {
                        j.HasKey("DocumentId", "EventId").HasName("PK_EventMaterial");
                        j.ToTable("EventDocument");
                    });
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

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Department");

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
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_EventType");

            entity.HasOne(d => d.ResponsibleEmployee).WithMany(p => p.EventsNavigation)
                .HasForeignKey(d => d.ResponsibleEmployeeId)
                .HasConstraintName("FK_Event_Employee");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("PK_EventName");

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
