using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace crimson_closet.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly dbContext _dbContext;

        public RoleController(RoleManager<IdentityRole> roleManager, dbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<ProjectRole> roleList = new List<ProjectRole>();

            // grabbing the AspNet roles from the database
            foreach(var role in _roleManager.Roles.ToList())
            {
                roleList.Add(new ProjectRole() { Id = new Guid(role.Id), RoleName = role.Name});
            }

            return View(roleList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectRole role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role.RoleName);
            if (!roleExists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Role already exists.");

            return View(role);
        }

    }
}
