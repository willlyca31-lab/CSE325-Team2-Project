
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CSE325_Team2_Project.Data;

// Extends ASP.NET Core Identity with additional profile information
// for members registered in the Novel Gatherings application.
public class ApplicationUser : IdentityUser
{
    // Stores the member's full name.
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    // Stores the member's residential or mailing address.
    [Required]
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;
}
