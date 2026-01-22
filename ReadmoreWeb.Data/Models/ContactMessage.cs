using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models;

public class ContactMessage
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Subject { get; set; } = string.Empty;

    [Required, MaxLength(4000)]
    public string Message { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "New"; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
