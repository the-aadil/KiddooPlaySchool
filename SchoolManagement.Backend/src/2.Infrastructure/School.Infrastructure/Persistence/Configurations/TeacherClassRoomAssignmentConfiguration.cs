using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class TeacherClassRoomAssignmentConfiguration : IEntityTypeConfiguration<TeacherClassRoomAssignment>
{
    public void Configure(EntityTypeBuilder<TeacherClassRoomAssignment> builder)
    {
        builder.ToTable("TeacherClassRoomAssignments");
        builder.HasKey(tc => new { tc.TeacherProfileId, tc.ClassRoomId });
        builder.Property(tc => tc.AssignmentDate)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");
        builder.HasOne(tc => tc.TeacherProfile)
            .WithMany(t => t.TeacherClassRoomAssignments)
            .HasForeignKey(tc => tc.TeacherProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(tc => tc.ClassRoom)
            .WithMany(c => c.TeacherClassRoomAssignments)
            .HasForeignKey(tc => tc.ClassRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
