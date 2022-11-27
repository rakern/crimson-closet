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
using Microsoft.AspNetCore.Authorization;

namespace crimson_closet.Controllers
{
    [Authorize]
    public class CustOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CustOrders/AllOrders
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllOrders()
        {
            var applicationDbContext = _context.CustOrder.Include(c => c.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CustOrders/CustomerOrders
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerOrders()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.CustOrder.Include(c => c.ApplicationUser).Where( i => i.ApplicationUserId == currentUser.Id);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: CustOrders/CheckoutConfirmationPage
        [Authorize(Roles = "Customer")]
        public IActionResult CheckoutConfirmationPage()
        {
            return View();
        }

        // POST: CustOrders/Checkout
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Checkout()
        {
            Console.WriteLine("\n\n\n\nghghghgh\n\n\n\n");
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            CustOrder custOrder = new CustOrder();
            if (ModelState.IsValid)
            {
                custOrder.Id = Guid.NewGuid();
                custOrder.ApplicationUserId = currentUser.Id;
                custOrder.CheckOutDate = DateTime.Now;
                custOrder.ReturnByDate = custOrder.CheckOutDate.AddDays(Constants.RETURN_BY_DAYS);
                try
                {
					_context.Add(custOrder);
					await _context.SaveChangesAsync();
                }
                catch
                {
                    return RedirectToAction("Error", "Home");
                }
                List<CartItem> cartItems = _context.CartItem.Where( i => i.ApplicationUserId== currentUser.Id ).Include(i => i.Item).ToList();
                foreach (CartItem cartItem in cartItems)
                {
                    CustOrderItem custOrderItem = new CustOrderItem();
                    custOrderItem.ItemId = cartItem.ItemId;
                    custOrderItem.CustOrderId = custOrder.Id;
					try
					{
						_context.Add(custOrderItem);
                        _context.Remove(cartItem);
						await _context.SaveChangesAsync();
					}
					catch
					{
						return RedirectToAction("Error", "Home");
					}
					
				}

				return View(nameof(CheckoutConfirmationPage), custOrder);

			}
            return View(custOrder);
        }

        // GET: CustOrders/Details/5
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.CustOrder == null)
            {
                return NotFound();
            }

            var custOrder = await _context.CustOrder
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custOrder == null)
            {
                return NotFound();
            }

            return View(custOrder);
        }

        // GET: CustOrders/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: CustOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,CheckOutDate,ReturnByDate")] CustOrder custOrder)
        {
            if (ModelState.IsValid)
            {
                custOrder.Id = Guid.NewGuid();
                _context.Add(custOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", custOrder.ApplicationUserId);
            return View(custOrder);
        }

        // GET: CustOrders/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.CustOrder == null)
            {
                return NotFound();
            }

            var custOrder = await _context.CustOrder.FindAsync(id);
            if (custOrder == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", custOrder.ApplicationUserId);
            return View(custOrder);
        }

        // POST: CustOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ApplicationUserId,CheckOutDate,ReturnByDate")] CustOrder custOrder)
        {
            if (id != custOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustOrderExists(custOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllOrders));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", custOrder.ApplicationUserId);
            return View(custOrder);
        }

        // GET: CustOrders/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CustOrder == null)
            {
                return NotFound();
            }

            var custOrder = await _context.CustOrder
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custOrder == null)
            {
                return NotFound();
            }

            return View(custOrder);
        }

        // POST: CustOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.CustOrder == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CustOrder'  is null.");
            }
            var custOrder = await _context.CustOrder.FindAsync(id);
            if (custOrder != null)
            {
                _context.CustOrder.Remove(custOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllOrders));
        }

        private bool CustOrderExists(Guid id)
        {
          return _context.CustOrder.Any(e => e.Id == id);
        }
    }
}
