using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        [Display(Name = "Bevestig wachtwoord")]
        public string ConfirmPassword { get; set; }
    }
}
