using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace crimson_closet.Controllers
{
    public class ApplicationRolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;



        public ApplicationRolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;

        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRole role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role.Name);
            if (!roleExists)
            {
                //role.name is used because Name is the property in IdentityRole that we need to set for the DB to update
                var result = await _roleManager.CreateAsync(new ApplicationRole(role.Name));
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Role already exists.");

            return View(role);
        }

    }
}
