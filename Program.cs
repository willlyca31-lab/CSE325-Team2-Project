
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSE325_Team2_Project.Components;
using CSE325_Team2_Project.Components.Account;
using CSE325_Team2_Project.Data;
using CSE325_Team2_Project.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Keep PostgreSQL configuration for the Neon database.
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Registers QuickGrid support.
builder.Services.AddQuickGridEntityFrameworkAdapter();

// Registers database services used by the membership and hosting pages.
builder.Services.AddScoped<MembershipService>();
builder.Services.AddScoped<HostingSignupService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// TEMPORARY: Test Neon database connection.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/api/test-db", async (IDbContextFactory<ApplicationDbContext> dbFactory) =>
    {
        try
        {
            // Creates a database context to test the connection.
            await using var db = await dbFactory.CreateDbContextAsync();

            bool connected = await db.Database.CanConnectAsync();

            return connected
                ? Results.Ok(new { message = "Successfully connected to Neon!" })
                : Results.Problem("Could not connect to Neon.");
        }
        catch
        {
            return Results.Problem("Database connection test failed.");
        }
    });
}

app.Run();
