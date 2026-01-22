using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

namespace ReadmoreWeb.Services.Contact
{
    public class ContactBadgeService : IContactBadgeService
    {
        private readonly ReadmoreDbContext _db;

        public ContactBadgeService(ReadmoreDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetUserUnreadCountAsync(string userId)
        {
            return await _db.ContactReplies
                .AsNoTracking()
                .Where(r =>
                    r.Sender == "Admin" &&
                    !r.SeenByUser &&
                    r.ContactMessage != null &&
                    r.ContactMessage.UserId == userId)
                .CountAsync();
        }

        public async Task<int> GetAdminUnreadCountAsync()
        {
            return await _db.ContactReplies
                .AsNoTracking()
                .Where(r =>
                    r.Sender == "User" &&
                    !r.SeenByAdmin)
                .CountAsync();
        }
    }
}
