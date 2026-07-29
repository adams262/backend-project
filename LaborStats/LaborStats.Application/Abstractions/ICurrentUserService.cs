namespace LaborStats.Application.Abstractions;

public interface ICurrentUserService
{
    string? GetUsername();
    Guid? GetUserId();
}