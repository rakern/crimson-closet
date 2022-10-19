using crimson_closet.Areas.Identity.Data;

namespace crimson_closet.Models
{
    public class DetailsUser_VM
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
