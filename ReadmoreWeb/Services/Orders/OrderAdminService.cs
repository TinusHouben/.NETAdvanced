using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

namespace ReadmoreWeb.Services.Orders;

public class OrderAdminService : IOrderAdminService
{
    private readonly ReadmoreDbContext _db;

    public OrderAdminService(ReadmoreDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _db.Orders.CountAsync(o => o.Status == "Pending");
    }
}
