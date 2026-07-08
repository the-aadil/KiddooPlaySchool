using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class AdminProfileConfiguration : IEntityTypeConfiguration<AdminProfile>
{
    public void Configure(EntityTypeBuilder<AdminProfile> builder)
    {
        builder.ToTable("AdminProfiles");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Department).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Designation).HasMaxLength(100);
        builder.HasOne(p => p.User).WithOne(u => u.AdminProfile)
            .HasForeignKey<AdminProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(p => p.UserId).IsUnique();
    }
}
