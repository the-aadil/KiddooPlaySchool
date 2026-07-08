using KiddooPlaySchool.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiddooPlaySchool.Infrastructure.Data.Configurations;

public class TeacherClassRoomConfiguration : IEntityTypeConfiguration<TeacherClassRoom>
{
    public void Configure(EntityTypeBuilder<TeacherClassRoom> builder)
    {
        builder.ToTable("TeacherClassRooms");

        builder.HasKey(tc => new { tc.TeacherProfileId, tc.ClassRoomId });

        builder.Property(tc => tc.AssignmentDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(tc => tc.TeacherProfile)
            .WithMany(t => t.TeacherClassRooms)
            .HasForeignKey(tc => tc.TeacherProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tc => tc.ClassRoom)
            .WithMany(c => c.TeacherClassRooms)
            .HasForeignKey(tc => tc.ClassRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
