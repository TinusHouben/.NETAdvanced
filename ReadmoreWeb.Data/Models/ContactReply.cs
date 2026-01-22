using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models;

public class ContactReply
{
    public int Id { get; set; }

    public int ContactMessageId { get; set; }
    public ContactMessage? ContactMessage { get; set; }

    [Required, MaxLength(3000)]
    public string Text { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string Sender { get; set; } = "User";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? SenderUserId { get; set; }

    public bool SeenByUser { get; set; } = false;
    public bool SeenByAdmin { get; set; } = false;
}
