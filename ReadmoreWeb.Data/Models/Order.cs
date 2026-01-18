using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    public ApplicationUser? User { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Created";

    public List<OrderItem> Items { get; set; } = new();
}
