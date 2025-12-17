using Microsoft.AspNetCore.Identity;

namespace ReadmoreWeb.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Extra properties (verplicht voor examen)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
