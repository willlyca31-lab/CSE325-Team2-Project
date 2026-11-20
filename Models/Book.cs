using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.SignalR;

namespace CSE325_Team2_Project.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Range(1000, 2100, ErrorMessage = "Please enter a year between 1000 and 2100.")]
    public int? PublishYear { get; set; }

    [StringLength(50)]
    public string? Genre { get; set; }

    public bool IsSelected { get; set; }
}