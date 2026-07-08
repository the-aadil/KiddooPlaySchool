namespace School.Application.Interfaces;

public interface ICurrentUserContext
{
    Guid UserId { get; }
    string UserName { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}
