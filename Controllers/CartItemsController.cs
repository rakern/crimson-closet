using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crimson_closet.Data;
using crimson_closet.Models;

namespace crimson_closet.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public CartItemsController(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            //Use Then include to get a sub object of the subobject
              return View(await _dbcontext.CartItems.Include(i => i.Cart).ThenInclude(j=>j.ApplicationUser).Include(i => i.Item).ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _dbcontext.CartItems == null)
            {
                return NotFound();
            }

            var cartItems = await _dbcontext.CartItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItems == null)
            {
                return NotFound();
            }

            return View(cartItems);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            //Adds to ViewBag all of the Carts and corresponding User with Id as the identifier and the UserName/Name as the display
            ViewData["CartId"] = new SelectList(_dbcontext.Cart.Include(i=>i.ApplicationUser), "Id", "ApplicationUser.LastName");
            //Adds to ViewBag all of the Carts and corresponding User with Id as the identifier and the UserName/Name as the display
            ViewData["ItemId"] = new SelectList(_dbcontext.Item, "ItemId", "ItemCode");
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,ItemId")] CartItems cartItems)
        {
            if (ModelState.IsValid)
            {
                cartItems.Id = Guid.NewGuid();
                _dbcontext.Add(cartItems);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cartItems);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _dbcontext.CartItems == null)
            {
                return NotFound();
            }

            var cartItems = await _dbcontext.CartItems.FindAsync(id);
            if (cartItems == null)
            {
                return NotFound();
            }
            return View(cartItems);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id")] CartItems cartItems)
        {
            if (id != cartItems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbcontext.Update(cartItems);
                    await _dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemsExists(cartItems.Id))
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
            return View(cartItems);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _dbcontext.CartItems == null)
            {
                return NotFound();
            }

            var cartItems = await _dbcontext.CartItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItems == null)
            {
                return NotFound();
            }

            return View(cartItems);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_dbcontext.CartItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CartItems'  is null.");
            }
            var cartItems = await _dbcontext.CartItems.FindAsync(id);
            if (cartItems != null)
            {
                _dbcontext.CartItems.Remove(cartItems);
            }
            
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemsExists(Guid id)
        {
          return _dbcontext.CartItems.Any(e => e.Id == id);
        }
    }
}
