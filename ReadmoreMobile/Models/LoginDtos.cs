namespace ReadmoreMobile.Models;

public sealed class LoginRequestDto
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public sealed class LoginResponseDto
{
    public string Token { get; set; } = "";
    public DateTime ExpiresAtUtc { get; set; }
    public string Email { get; set; } = "";
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}
