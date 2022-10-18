using crimson_closet.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace crimson_closet.Controllers
{
    public class UserRoles : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BasicUser> _userManager;


        public UserRoles(RoleManager<IdentityRole> roleManager, UserManager<BasicUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<UserRoles> userRolesList = new List<UserRoles>();
            return View();
        }
    }
}
