namespace ReadmoreMobile.Models;

public sealed record LoginRequest(string Email, string Password);

public sealed record LoginResponse(string Token, DateTime ExpiresAtUtc);
