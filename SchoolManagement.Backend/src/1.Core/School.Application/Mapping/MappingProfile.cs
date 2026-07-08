using AutoMapper;
using School.Application.DTOs.Attendance;
using School.Application.DTOs.ClassRooms;
using School.Application.DTOs.DailyLogs;
using School.Domain.Entities;

namespace School.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClassRoom, ClassRoomResponse>()
            .ForMember(d => d.AgeGroup, o => o.MapFrom(s => s.AgeGroup.ToString()))
            .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.Students.Count(st => !st.IsDeleted)));

        CreateMap<AttendanceRecord, AttendanceRecordResponse>()
            .ForMember(d => d.StudentName, o => o.MapFrom(s =>
                s.Student.User.FirstName + " " + s.Student.User.LastName))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<DailyActivityLog, DailyLogResponse>()
            .ForMember(d => d.StudentName, o => o.MapFrom(s =>
                s.Student.User.FirstName + " " + s.Student.User.LastName))
            .ForMember(d => d.TeacherName, o => o.MapFrom(s =>
                s.Teacher.User.FirstName + " " + s.Teacher.User.LastName))
            .ForMember(d => d.ActivityType, o => o.MapFrom(s => s.ActivityType.ToString()))
            .ForMember(d => d.Visibility, o => o.MapFrom(s => s.Visibility.ToString()))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt));
    }
}
