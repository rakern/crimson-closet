using crimson_closet.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using crimson_closet.Models;

namespace crimson_closet.Data;

public class dbContext : IdentityDbContext<BasicUser>
{
    public dbContext(DbContextOptions<dbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<crimson_closet.Models.CrimsonClosetUserForDisplay> CrimsonClosetUserForDisplay { get; set; }

}
