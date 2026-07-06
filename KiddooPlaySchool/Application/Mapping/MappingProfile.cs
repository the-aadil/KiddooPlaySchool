using AutoMapper;
using KiddooPlaySchool.Application.DTOs.ClassRoom;
using KiddooPlaySchool.Application.DTOs.Student;
using KiddooPlaySchool.Application.DTOs.Teacher;
using KiddooPlaySchool.Domain.Entities;

namespace KiddooPlaySchool.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeacherProfile, TeacherResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.User.MobileNumber))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<StudentProfile, StudentResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.MobileNumber))
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate))
            .ForMember(dest => dest.ClassRoomName, opt => opt.MapFrom(src => src.ClassRoom != null ? src.ClassRoom.Name : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<ClassRoom, ClassRoomResponse>()
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count(s => !s.IsDeleted)));
    }
}
