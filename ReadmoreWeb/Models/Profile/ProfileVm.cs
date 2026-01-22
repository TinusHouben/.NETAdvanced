using System.ComponentModel.DataAnnotations;

namespace ReadmoreWeb.Models.Profile;

public class ProfileVm
{
    [Required(ErrorMessage = "Voornaam is verplicht.")]
    [StringLength(50)]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Achternaam is verplicht.")]
    [StringLength(50)]
    public string LastName { get; set; } = "";

    [Phone(ErrorMessage = "Ongeldig telefoonnummer.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Straat en nummer is verplicht.")]
    [StringLength(100)]
    public string Street { get; set; } = "";

    [Required(ErrorMessage = "Stad is verplicht.")]
    [StringLength(80)]
    public string City { get; set; } = "";

    [Required(ErrorMessage = "Postcode is verplicht.")]
    [StringLength(20)]
    public string PostalCode { get; set; } = "";
}
