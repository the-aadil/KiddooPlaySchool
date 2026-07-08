using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Exceptions;

namespace School.Application.Features.ClassRooms.Commands;

public class AssignTeacherCommandHandler : IRequestHandler<AssignTeacherCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AssignTeacherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AssignTeacherCommand request, CancellationToken cancellationToken)
    {
        var classRoom = await _context.ClassRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ClassRoomId && !c.IsDeleted, cancellationToken)
            ?? throw new DomainException($"ClassRoom with ID {request.ClassRoomId} was not found.");

        var teacher = await _context.TeacherProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.TeacherProfileId && !t.IsDeleted, cancellationToken)
            ?? throw new DomainException($"TeacherProfile with ID {request.TeacherProfileId} was not found.");

        var alreadyAssigned = await _context.TeacherClassRoomAssignments
            .AnyAsync(tc => tc.TeacherProfileId == request.TeacherProfileId
                         && tc.ClassRoomId == request.ClassRoomId, cancellationToken);

        if (alreadyAssigned)
        {
            throw new DomainException("Teacher is already assigned to this classroom.");
        }

        _context.TeacherClassRoomAssignments.Add(new TeacherClassRoomAssignment
        {
            TeacherProfileId = request.TeacherProfileId,
            ClassRoomId = request.ClassRoomId,
            AssignmentDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
