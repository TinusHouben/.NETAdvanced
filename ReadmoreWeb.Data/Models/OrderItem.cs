using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order? Order { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book? Book { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; } = 1;

        [Range(0, 999999)]
        public decimal UnitPrice { get; set; }
    }
}
