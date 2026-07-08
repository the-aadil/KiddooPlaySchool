using KiddooPlaySchool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiddooPlaySchool.Infrastructure.Data.Configurations;

public class DailyActivityLogConfiguration : IEntityTypeConfiguration<DailyActivityLog>
{
    public void Configure(EntityTypeBuilder<DailyActivityLog> builder)
    {
        builder.ToTable("DailyActivityLogs");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.LogTimestamp)
            .IsRequired();

        builder.Property(d => d.ActivityType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.Payload)
            .HasMaxLength(2000);

        builder.Property(d => d.Visibility)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.Remarks)
            .HasMaxLength(1000);

        builder.Property(d => d.MediaUrls)
            .HasMaxLength(2000);

        builder.Property(d => d.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasOne(d => d.Student)
            .WithMany()
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Teacher)
            .WithMany()
            .HasForeignKey(d => d.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => new { d.StudentId, d.LogTimestamp })
            .HasFilter("IsDeleted = 0");

        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}
