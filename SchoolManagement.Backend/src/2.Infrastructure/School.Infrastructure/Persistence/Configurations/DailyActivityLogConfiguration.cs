using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class DailyActivityLogConfiguration : IEntityTypeConfiguration<DailyActivityLog>
{
    public void Configure(EntityTypeBuilder<DailyActivityLog> builder)
    {
        builder.ToTable("DailyActivityLogs");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.LogTimestamp).IsRequired();
        builder.Property(d => d.ActivityType).HasConversion<int>().IsRequired();
        builder.Property(d => d.Payload).HasMaxLength(2000);
        builder.Property(d => d.Visibility).HasConversion<int>().IsRequired();
        builder.Property(d => d.Remarks).HasMaxLength(1000);

        var jsonOptions = default(System.Text.Json.JsonSerializerOptions);

        builder.Property(d => d.MediaUrls)
            .HasColumnType("nvarchar(max)")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode(StringComparison.Ordinal))),
                c => c.ToList()));

        builder.HasOne(d => d.Student)
            .WithMany()
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Teacher)
            .WithMany()
            .HasForeignKey(d => d.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => new { d.StudentId, d.LogTimestamp });
    }
}
