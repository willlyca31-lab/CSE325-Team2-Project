
using System.ComponentModel.DataAnnotations;
using CSE325_Team2_Project.Data;

namespace CSE325_Team2_Project.Models;

// Connects an Identity user to a specific book club.
public class Membership
{
    public int Id { get; set; }

    [Required]
    public int ClubId { get; set; }

    public Club Club { get; set; } = default!;

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = default!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Active";
}
