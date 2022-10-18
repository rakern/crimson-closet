using Microsoft.AspNetCore.Identity;

namespace crimson_closet.Areas.Identity.Data
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string roleName)
        {
            Name = roleName;
        }
        public ApplicationRole()
        {

        }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
