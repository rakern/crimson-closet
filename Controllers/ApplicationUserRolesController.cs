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
            //Adds to ViewBag all of the Users and Roles with Id as the identifier and the UserName/Name as the display
            ViewData["UserId"] = new SelectList(_dbcontext.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_dbcontext.Roles, "Id", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUserRole userRole)
        {
            //Check to see if there is already a combo of User and Role like this already in database.
            var search = _dbcontext.ApplicationUserRoles.Include(c => c.User).Include(c => c.Role)
                .ToList().Where(c => (c.User.Id == userRole.UserId.ToString() && c.Role.Id == userRole.RoleId.ToString()));
          
            
            if (search.Count() == 0)
            {
                //The userRole parameter comes into this method with only the Foreign Keys: UserID and RoleID
                //We Then have to go get that specific user and role because we have to pass the role name and
                //the full ApplicationUser object into the function "AddToRoleAsync"
                //we use .ToList()[0]; in order to convert the query result into the class ApplicationRole and UserRole
                userRole.User = _userManager.Users.Where(c => c.Id == userRole.UserId.ToString()).ToList()[0];
                userRole.Role = _dbcontext.Roles.Where(c => c.Id == userRole.RoleId.ToString()).ToList()[0];

                //Adds the role to the user
                var result = await _userManager.AddToRoleAsync(userRole.User, userRole.Role.Name);
                return RedirectToAction(nameof(Index)); //means success if we get to here
            }

            ModelState.AddModelError(string.Empty, "User Role combo already exists.");

            ViewData["UserId"] = new SelectList(_dbcontext.Users, "Id", "UserName", userRole.UserId);
            ViewData["RoleId"] = new SelectList(_dbcontext.Roles, "Id", "Name", userRole.RoleId);

            return View(userRole);
        }
    }
}
