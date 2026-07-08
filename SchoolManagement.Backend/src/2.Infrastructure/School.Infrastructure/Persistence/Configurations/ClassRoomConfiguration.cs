using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
{
    public void Configure(EntityTypeBuilder<ClassRoom> builder)
    {
        builder.ToTable("ClassRooms");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.AgeGroup).HasConversion<int>().IsRequired();
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.HasIndex(c => c.Name)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
    }
}
