using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

namespace ReadmoreWeb.Services.Contact
{
    public class ContactAdminService : IContactAdminService
    {
        private readonly ReadmoreDbContext _db;

        public ContactAdminService(ReadmoreDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetNewCountAsync()
        {
            return await _db.ContactMessages
                .AsNoTracking()
                .CountAsync(m => m.Status == "New");
        }
    }
}
