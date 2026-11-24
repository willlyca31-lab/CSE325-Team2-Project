
using CSE325_Team2_Project.Data;
using CSE325_Team2_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace CSE325_Team2_Project.Services;

// Handles database operations for book club hosting signups.
public class HostingSignupService
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public HostingSignupService(
        IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
    }

    // Retrieves books approved for hosting.
    public async Task<List<Book>> GetApprovedBooksAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Books
            .AsNoTracking()
            .Where(b => b.IsApproved)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    // Retrieves all hosting registrations for a particular club.
    public async Task<List<HostingSignup>> GetHostingSignupsAsync(int clubId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.HostingSignups
            .AsNoTracking()
            .Include(h => h.Book)
            .Include(h => h.HostUser)
            .Include(h => h.Club)
            .Where(h => h.ClubId == clubId)
            .OrderBy(h => h.HostingDate)
            .ToListAsync();
    }

    // Creates a hosting signup after validating membership and book approval.
    public async Task CreateHostingSignupAsync(HostingSignup signup)
    {
        await using var db = await _factory.CreateDbContextAsync();

        bool isMember = await db.Memberships.AnyAsync(m =>
            m.ClubId == signup.ClubId &&
            m.UserId == signup.HostUserId &&
            m.Status == "Active");

        if (!isMember)
        {
            throw new InvalidOperationException(
                "You must be an active club member to host a meeting.");
        }

        bool bookApproved = await db.Books.AnyAsync(b =>
            b.Id == signup.BookId && b.IsApproved);

        if (!bookApproved)
        {
            throw new InvalidOperationException(
                "The selected book has not been approved.");
        }

        bool clubExists = await db.Clubs
            .AnyAsync(c => c.Id == signup.ClubId);

        if (!clubExists)
        {
            throw new InvalidOperationException(
                "The selected book club does not exist.");
        }

        if (signup.HostingDate.Date < DateTime.Today)
        {
            throw new InvalidOperationException(
                "The hosting date cannot be in the past.");
        }

        db.HostingSignups.Add(signup);

        await db.SaveChangesAsync();
    }
}
