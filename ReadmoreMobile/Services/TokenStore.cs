namespace ReadmoreMobile.Services;

public sealed class TokenStore
{
    private const string TokenKey = "jwt_token";
    private const string ExpKey = "jwt_exp";

    public async Task SaveAsync(string token, DateTime expiresAtUtc)
    {
        await SecureStorage.SetAsync(TokenKey, token);
        await SecureStorage.SetAsync(ExpKey, expiresAtUtc.ToString("O"));
    }

    public async Task<(string? Token, DateTime? ExpiresAtUtc)> GetAsync()
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        var exp = await SecureStorage.GetAsync(ExpKey);

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(exp))
            return (null, null);

        if (!DateTime.TryParse(exp, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
            return (null, null);

        return (token, dt);
    }

    public void Clear()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(ExpKey);
    }
}
