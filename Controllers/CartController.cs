using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crimson_closet.Data;
using crimson_closet.Models;
using crimson_closet.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace crimson_closet.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = context;
            _userManager = userManager;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            
              return View(await _dbcontext.Cart.Include(i=>i.ApplicationUser).ToListAsync());
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _dbcontext.Cart == null)
            {
                return NotFound();
            }

            var cart = await _dbcontext.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Cart/Create
        public IActionResult Create()
        {
            //Adds to ViewBag all of the Users with Id as the identifier and the UserName/Name as the display
            ViewData["UserId"] = new SelectList(_dbcontext.Users, "Id", "UserName");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,CreatedDate,ExpiredDate")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                cart.Id = Guid.NewGuid();
                _dbcontext.Add(cart);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _dbcontext.Cart == null)
            {
                return NotFound();
            }

            var cart = await _dbcontext.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CreatedDate,ExpiredDate")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbcontext.Update(cart);
                    await _dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _dbcontext.Cart == null)
            {
                return NotFound();
            }

            var cart = await _dbcontext.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        //We pass in both the UserId and RoleId to know which record to delete since they are both together the primary keys
        [Route("/Cart/Delete/:CartId", Name = "deleteCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid CartId)
        {
            if (_dbcontext.Cart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _dbcontext.Cart.FindAsync(CartId);
            if (cart != null)
            {
                _dbcontext.Cart.Remove(cart);
            }
            
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(Guid id)
        {
          return _dbcontext.Cart.Any(e => e.Id == id);
        }
    }
}
