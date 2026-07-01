using KiddooPlaySchool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiddooPlaySchool.Infrastructure.Data.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FirstName)
            .HasMaxLength(50).IsRequired();

        builder.Property(t => t.LastName)
            .HasMaxLength(50).IsRequired();

        builder.Property(t => t.Username)
            .HasMaxLength(30).IsRequired();

        builder.HasIndex(t => t.Username).IsUnique();

        builder.Property(t => t.Email)
            .HasMaxLength(100).IsRequired();

        builder.HasIndex(t => t.Email).IsUnique();

        builder.Property(t => t.PasswordHash)
            .HasMaxLength(200).IsRequired();

        builder.Property(t => t.MobileNumber)
            .HasMaxLength(15).IsRequired();

        builder.HasIndex(t => t.MobileNumber).IsUnique();

        builder.Property(t => t.AlternateMobile)
            .HasMaxLength(15);

        builder.Property(t => t.DateOfBirth)
            .IsRequired();

        builder.Property(t => t.Gender)
            .HasMaxLength(10).IsRequired();

        builder.Property(t => t.Qualification)
            .HasMaxLength(100).IsRequired();

        builder.Property(t => t.Specialization)
            .HasMaxLength(100);

        builder.Property(t => t.Address)
            .HasMaxLength(200);

        builder.Property(t => t.City)
            .HasMaxLength(50);

        builder.Property(t => t.State)
            .HasMaxLength(50);

        builder.Property(t => t.JoinDate)
            .IsRequired();

        builder.Property(t => t.EmployeeId)
            .HasMaxLength(20).IsRequired();

        builder.HasIndex(t => t.EmployeeId).IsUnique();

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
