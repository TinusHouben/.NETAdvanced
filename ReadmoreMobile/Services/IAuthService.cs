namespace ReadmoreMobile.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
}
