
using CSE325_Team2_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSE325_Team2_Project.Data;

// Main database context for Novel Gatherings.
// Extends ASP.NET Core Identity to manage application users.
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    // Stores all books registered in the application.
    public DbSet<Book> Books { get; set; } = default!;

    // Stores the available book clubs.
    public DbSet<Club> Clubs { get; set; } = default!;

    // Stores memberships connecting users to book clubs.
    public DbSet<Membership> Memberships { get; set; } = default!;

    // Stores hosting registration requests.
    public DbSet<HostingSignup> HostingSignups { get; set; } = default!;

    // Configures database relationships, constraints, and initial data.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Preserve all existing ASP.NET Core Identity configurations.
        base.OnModelCreating(modelBuilder);

        // A user can only have one membership in each book club.
        modelBuilder.Entity<Membership>()
            .HasIndex(m => new { m.ClubId, m.UserId })
            .IsUnique();

        // Connects each membership to its book club.
        modelBuilder.Entity<Membership>()
            .HasOne(m => m.Club)
            .WithMany()
            .HasForeignKey(m => m.ClubId)
            .OnDelete(DeleteBehavior.Cascade);

        // Connects each membership to its registered application user.
        modelBuilder.Entity<Membership>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Connects hosting registrations to their respective clubs.
        modelBuilder.Entity<HostingSignup>()
            .HasOne(h => h.Club)
            .WithMany()
            .HasForeignKey(h => h.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        // Connects hosting registrations to their hosts.
        modelBuilder.Entity<HostingSignup>()
            .HasOne(h => h.HostUser)
            .WithMany()
            .HasForeignKey(h => h.HostUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Connects hosting registrations to selected books.
        modelBuilder.Entity<HostingSignup>()
            .HasOne(h => h.Book)
            .WithMany()
            .HasForeignKey(h => h.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // Creates the default Novel Gatherings book club.
        modelBuilder.Entity<Club>().HasData(
            new Club
            {
                Id = 1,
                Name = "Novel Gatherings",
                Description = "The main Novel Gatherings book club.",
                CreatedAt = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
