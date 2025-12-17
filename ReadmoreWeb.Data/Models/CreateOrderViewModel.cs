using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Models
{
    public class CreateOrderViewModel
    {
        [Required]
        public int BookId { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; } = 1;
    }
}
