using AutoMapper;
using KiddooPlaySchool.Application.DTOs;
using KiddooPlaySchool.Domain.Entities;

namespace KiddooPlaySchool.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
