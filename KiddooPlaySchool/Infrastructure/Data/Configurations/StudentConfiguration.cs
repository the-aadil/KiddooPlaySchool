using KiddooPlaySchool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiddooPlaySchool.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.DateOfBirth)
            .IsRequired();

        builder.Property(s => s.Phone)
            .HasMaxLength(20);

        builder.Property(s => s.Address)
            .HasMaxLength(200);

        builder.Property(s => s.ParentName)
            .HasMaxLength(100);

        builder.Property(s => s.ParentPhone)
            .HasMaxLength(20);

        builder.Property(s => s.ParentEmail)
            .HasMaxLength(100);

        builder.Property(s => s.EnrollmentDate)
            .IsRequired();

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
