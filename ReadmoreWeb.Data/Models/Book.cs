using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    [Range(0, 1000)]
    public decimal Price { get; set; }

    public DateTime PublishedDate { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
}
