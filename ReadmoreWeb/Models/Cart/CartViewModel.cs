namespace ReadmoreWeb.Models.Cart;

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    public int Count => Items.Sum(i => i.Quantity);
}
