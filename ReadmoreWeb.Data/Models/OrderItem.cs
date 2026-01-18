using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    [Range(1, 999)]
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }
}
