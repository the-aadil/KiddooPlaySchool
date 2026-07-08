using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUsers");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(200).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.MobileNumber).HasMaxLength(20).IsRequired();
        builder.Property(u => u.Role).HasMaxLength(20).IsRequired();
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
    }
}
