using MediatR;
using School.Application.DTOs.ClassRooms;
using School.Domain.Enums;

namespace School.Application.Features.ClassRooms.Commands;

public class CreateClassRoomCommand : IRequest<ClassRoomResponse>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public AgeGroup AgeGroup { get; set; }
}
