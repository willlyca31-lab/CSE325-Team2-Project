
using System.ComponentModel.DataAnnotations;
using CSE325_Team2_Project.Data;

namespace CSE325_Team2_Project.Models;

// Records a member's intention to host a book club meeting.
public class HostingSignup
{
    public int Id { get; set; }

    [Required]
    public int ClubId { get; set; }

    public Club Club { get; set; } = default!;

    [Required]
    public string HostUserId { get; set; } = string.Empty;

    public ApplicationUser HostUser { get; set; } = default!;

    [Required]
    public int BookId { get; set; }

    public Book Book { get; set; } = default!;

    [Required]
    public DateTime HostingDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
