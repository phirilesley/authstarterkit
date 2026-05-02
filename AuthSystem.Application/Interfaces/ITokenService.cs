namespace AuthSystem.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    DateTime GetAccessTokenExpiryUtc();
}
