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
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Authorization;

namespace crimson_closet.Controllers
{
    [Authorize]
    public class CartItemController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartItemController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = context;
            _userManager = userManager;
        }

        
        
        // GET: Static page for no cart
        public IActionResult NoCartPage()
        {
            
              return View();
        }
        
        // GET: Gets the page for all of the customer specific cart with items in it
        public async Task<IActionResult> CustomerCart()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);//Gets current User
            List<CartItem> cart_items = _dbcontext.CartItem
                .Include(i => i.ApplicationUser).Where(i => i.ApplicationUserId == currentUser.Id)
                .Include(i => i.Item).ThenInclude(i => i.ItemType).ToList();
            if (cart_items.Count == 0)
            {
                return RedirectToAction(nameof(NoCartPage));
            }
            //send in list of cartitems for the signned in user
            return View(cart_items);
        }

       
        // GET: Cart/Create
        public IActionResult Create()
        {
            //Adds to ViewBag all of the Users with Id as the identifier and the UserName/Name as the display
            ViewData["UserId"] = new SelectList(_dbcontext.Users, "Id", "UserName");
            ViewData["ItemId"] = new SelectList(_dbcontext.Item, "ItemId", "ItemCode");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,ItemId")] CartItem cart)
        {
            

            if (ModelState.IsValid)
            {
                cart.Id = Guid.NewGuid();
                _dbcontext.Add(cart);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerCart));
            }
            return View(cart);
        }

       
        

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _dbcontext.CartItem == null)
            {
                return NotFound();
            }

            var cart = await _dbcontext.CartItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        //We pass in both the CartId to know which record to delete since they are both together the primary keys
        [Route("/Cart/Delete/:CartItemId", Name = "deleteCartItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid CartItemId)
        {
            if (_dbcontext.CartItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _dbcontext.CartItem.FindAsync(CartItemId);
            if (cart != null)
            {
                _dbcontext.CartItem.Remove(cart);
            }
            
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(CustomerCart));
        }

        private bool CartExists(Guid id)
        {
          return _dbcontext.CartItem.Any(e => e.Id == id);
        }
    }
}
