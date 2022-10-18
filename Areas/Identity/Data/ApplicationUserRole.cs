using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Areas.Identity.Data
{
    public class ApplicationUserRole: IdentityUserRole<string>
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ApplicationUser User;

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public ApplicationRole Role;
    }
}
