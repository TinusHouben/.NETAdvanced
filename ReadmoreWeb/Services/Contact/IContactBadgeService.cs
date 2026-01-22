namespace ReadmoreWeb.Services.Contact
{
    public interface IContactBadgeService
    {
        Task<int> GetUserUnreadCountAsync(string userId);
        Task<int> GetAdminUnreadCountAsync();
    }
}
