using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Domain.Common;
using School.Domain.Entities;
using School.Infrastructure.Persistence.Interceptors;

namespace School.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntitySaveChangesInterceptor auditInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<AdminProfile> AdminProfiles => Set<AdminProfile>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public DbSet<TeacherClassRoomAssignment> TeacherClassRoomAssignments => Set<TeacherClassRoomAssignment>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<DailyActivityLog> DailyActivityLogs => Set<DailyActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseAuditableEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var eb = modelBuilder.Entity(entityType.ClrType);

            eb.Property<string>(nameof(BaseAuditableEntity.CreatedBy))
                .HasMaxLength(100)
                .IsRequired();

            eb.Property<string>(nameof(BaseAuditableEntity.LastModifiedBy))
                .HasMaxLength(100);

            var param = Expression.Parameter(entityType.ClrType, "e");
            var isDeleted = Expression.Property(param, "IsDeleted");
            var notDeleted = Expression.Not(isDeleted);
            var lambda = Expression.Lambda(notDeleted, param);
            eb.HasQueryFilter(lambda);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }
}
