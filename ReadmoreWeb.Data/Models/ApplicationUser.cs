using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Extra properties (verplicht voor examen)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(80)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }
    }
}
