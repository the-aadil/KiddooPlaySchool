using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Date).IsRequired();
        builder.Property(a => a.Status).HasConversion<int>().IsRequired();
        builder.Property(a => a.Notes).HasMaxLength(500);

        builder.HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.RecordedBy)
            .WithMany()
            .HasForeignKey(a => a.RecordedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.StudentId, a.Date })
            .IsUnique()
            .HasFilter("IsDeleted = 0");
    }
}
