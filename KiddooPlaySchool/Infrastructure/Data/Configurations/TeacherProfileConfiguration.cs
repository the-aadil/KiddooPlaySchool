using KiddooPlaySchool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiddooPlaySchool.Infrastructure.Data.Configurations;

public class TeacherProfileConfiguration : IEntityTypeConfiguration<TeacherProfile>
{
    public void Configure(EntityTypeBuilder<TeacherProfile> builder)
    {
        builder.ToTable("TeacherProfiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.AlternateMobile)
            .HasMaxLength(20);

        builder.Property(p => p.Gender)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.Qualification)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Specialization)
            .HasMaxLength(200);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.City)
            .HasMaxLength(100);

        builder.Property(p => p.State)
            .HasMaxLength(100);

        builder.Property(p => p.EmployeeId)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasOne(p => p.User)
            .WithOne(u => u.TeacherProfile)
            .HasForeignKey<TeacherProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.HasIndex(p => p.EmployeeId)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
