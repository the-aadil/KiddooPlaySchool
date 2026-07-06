using KiddooPlaySchool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiddooPlaySchool.Infrastructure.Data.Configurations;

public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.ToTable("StudentProfiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.ParentName)
            .HasMaxLength(100);

        builder.Property(p => p.ParentPhone)
            .HasMaxLength(20);

        builder.Property(p => p.ParentEmail)
            .HasMaxLength(100);

        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasOne(p => p.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<StudentProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.ClassRoom)
            .WithMany(c => c.Students)
            .HasForeignKey(p => p.ClassRoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.HasIndex(p => p.ClassRoomId)
            .HasFilter("ClassRoomId IS NOT NULL AND IsDeleted = 0");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
