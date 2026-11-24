
using CSE325_Team2_Project.Data;
using CSE325_Team2_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace CSE325_Team2_Project.Services;

// Contains database functions for managing book club memberships.
public class MembershipService
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public MembershipService(
        IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
    }

    // Retrieves all available book clubs.
    public async Task<List<Club>> GetClubsAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Clubs
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    // Retrieves active members belonging to a specific club.
    public async Task<List<Membership>> GetMembershipsAsync(int clubId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Memberships
            .AsNoTracking()
            .Include(m => m.User)
            .Include(m => m.Club)
            .Where(m => m.ClubId == clubId && m.Status == "Active")
            .OrderBy(m => m.User.FullName)
            .ToListAsync();
    }

    // Checks whether a user already belongs to a club.
    public async Task<bool> IsMemberAsync(int clubId, string userId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Memberships.AnyAsync(m =>
            m.ClubId == clubId &&
            m.UserId == userId &&
            m.Status == "Active");
    }

    // Adds a user to a club without creating duplicate memberships.
    public async Task AddMembershipAsync(int clubId, string userId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        bool exists = await db.Memberships.AnyAsync(m =>
            m.ClubId == clubId && m.UserId == userId);

        if (exists)
            return;

        db.Memberships.Add(new Membership
        {
            ClubId = clubId,
            UserId = userId,
            Status = "Active"
        });

        await db.SaveChangesAsync();
    }

    // Removes a membership record from the database.
    public async Task RemoveMembershipAsync(int membershipId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var membership = await db.Memberships
            .FirstOrDefaultAsync(m => m.Id == membershipId);

        if (membership is null)
            return;

        db.Memberships.Remove(membership);
        await db.SaveChangesAsync();
    }
}
