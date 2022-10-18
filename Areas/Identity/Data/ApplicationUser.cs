using Microsoft.AspNetCore.Identity;

namespace crimson_closet.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BasicUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}

