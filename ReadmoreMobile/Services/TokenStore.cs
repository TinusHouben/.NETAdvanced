namespace ReadmoreMobile.Services;

public class TokenStore
{
    private const string TokenKey = "jwt_token";

    public Task SaveAsync(string token)
    {
        return SecureStorage.SetAsync(TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(TokenKey);
        }
        catch
        {
            return null;
        }
    }

    public void Clear()
    {
        SecureStorage.Remove(TokenKey);
    }
}
