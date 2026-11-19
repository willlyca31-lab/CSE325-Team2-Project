using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.SignalR;

namespace CSE325_Team2_Project.Models;

public class Book
{
    public int Id { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }

    public DateOnly PublishDate { get; set; }

    public string? Genre { get; set; }

    public bool IsSelected { get; set; }
}