namespace AuthSystem.Application.Interfaces;

public interface IUserRegistrationService
{
    Task<UserRegistrationResult> RegisterUserAsync(string email, string password, string fullName, CancellationToken cancellationToken = default);
    Task<bool> ConfirmEmailAsync(string email, string confirmationToken, CancellationToken cancellationToken = default);
}

public sealed class UserRegistrationResult
{
    public bool Success { get; init; }
    public Guid UserId { get; init; }
    public string? ConfirmationToken { get; init; }
}
