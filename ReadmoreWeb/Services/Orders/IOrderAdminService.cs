namespace ReadmoreWeb.Services.Orders;

public interface IOrderAdminService
{
    Task<int> GetPendingCountAsync();
}
