using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.ToTable("StudentProfiles");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Address).HasMaxLength(500);
        builder.Property(p => p.ParentName).HasMaxLength(100);
        builder.Property(p => p.ParentPhone).HasMaxLength(20);
        builder.Property(p => p.ParentEmail).HasMaxLength(100);
        builder.HasOne(p => p.User).WithOne(u => u.StudentProfile)
            .HasForeignKey<StudentProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(p => p.ClassRoom).WithMany(c => c.Students)
            .HasForeignKey(p => p.ClassRoomId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
    }
}
