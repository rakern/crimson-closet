using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace crimson_closet.Controllers
{
    public class ApplicationUserRolesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManger; 
        private readonly RoleManager<ApplicationRole> _userRoleManger; 

        public ApplicationUserRolesController(ILogger<HomeController> logger,
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
            //QUeries for all UserRole instances in table
            var userRoles = _dbcontext.ApplicationUserRoles.Include(c => c.User).Include(c => c.Role).ToList();
       
            return View(userRoles);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_dbcontext.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_dbcontext.Roles, "Id", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUserRole userRole)
        {
            //var userRoleExists = _dbcontext.ApplicationUserRoles.Include(c => c.User)
            //    .Include(c => c.Role).Any(o => (o.UserId.ToString() == userRole.User.Id && o.RoleId.ToString() == userRole.Role.Id));
            Console.WriteLine(_dbcontext.ApplicationUserRoles);
            if (false)
            {
                //role.name is used because Name is the property in IdentityRole that we need to set for the DB to update
                var result = await _userManager.AddToRoleAsync(userRole.User, userRole.Role.Name);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "User Role combo already exists.");

            return View(userRole);
        }
    }
}
