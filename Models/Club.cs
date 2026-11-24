
using System.ComponentModel.DataAnnotations;

namespace CSE325_Team2_Project.Models;

// Represents a book club in Novel Gatherings.
public class Club
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
