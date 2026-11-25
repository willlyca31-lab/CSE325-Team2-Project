using Microsoft.AspNetCore.Identity;

namespace CSE325_Team2_Project.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser //This means ApplicationUser inherits all properties provided by ASP.NET Core Identity such as Email, PasswordHash, UserName, etc. and we can add our own properties to this class as needed.
{
    // Add additional properties for user profile information that are not included in IdentityUser.
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    
    //Need street address for meeting locations
    public string StreetAddress { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
}

