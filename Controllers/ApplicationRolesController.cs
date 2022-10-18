using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace crimson_closet.Controllers
{
    public class ApplicationRolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _dbcontext;



        public ApplicationRolesController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext dbcontext)
        {
            _roleManager = roleManager;
            _dbcontext = dbcontext;
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

        // GET: ApplicationRoles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _roleManager.Roles == null)
            {
                return NotFound();
            }

            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_roleManager.Roles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Actor'  is null.");
            }
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role != null)
            {
                _dbcontext.Roles.Remove(role);
                
            }

            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _roleManager == null)
            {
                return NotFound();
            }

            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            //changed to throw the role with a list of movies
            
            return View(role);
        }

    }
}
