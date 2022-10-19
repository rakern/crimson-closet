using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace crimson_closet.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManger;


        public ApplicationUsersController(ILogger<HomeController> logger,
         UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager,
         ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _dbcontext = applicationDbContext;
            _userManager = userManager;
            _roleManger = roleManager;

        }

        public IActionResult Index()
        {
            //some simple error handling or else it will blow up if they are not connected to db
            try
            {
                var users = _userManager.Users.ToList();
                return View(users);
            }
            catch
            {
                return View(new List<ApplicationUser>());
            }

        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _userManager == null)
            {
                return NotFound();
            }

            //Get the user from id
            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            //get a list of strings of the roles
            var roleListString = await _userManager.GetRolesAsync(user);
            //fill a custom view model that has been created from the default display view
            DetailsUser_VM detailsUser_VM = new DetailsUser_VM();
            detailsUser_VM.User = user;
            detailsUser_VM.Roles = roleListString;

            if (user == null)
            {
                return NotFound();
            }

            return View(detailsUser_VM);
        }



    }
}
