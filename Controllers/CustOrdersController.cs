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
using MailKit.Search;

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
          // GET: CustOrders/ActiveOrders
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ActiveOrders()
        {
            var applicationDbContext = _context.CustOrder.Include(c => c.ApplicationUser).Where(i => i.Status.ToLower() == "active");
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: CustOrders/AllOverdueOrders
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllOverdueOrders()
        {
            var applicationDbContext = _context.CustOrder.Include(c => c.ApplicationUser).Where( i => i.ReturnByDate < DateTime.Now);
            return View(await applicationDbContext.ToListAsync());
        }

        
        // GET: CustOrders/AllPendingOrders
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllPendingOrders()
        {
            var applicationDbContext = _context.CustOrder.Include(c => c.ApplicationUser).Where( i => i.Status.ToLower() == "pending");
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
                        cartItem.Item.ItemStatus = ItemStatus.Borrowed;
                        _context.Update(cartItem.Item);
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

        
        [HttpPost, ActionName("ActivateOrder")]
        [Route("/CustOrder/ActivateOrder/:OrderId", Name = "ActivateOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateOrder(Guid OrderId)
        {
           
            try
            {
                CustOrder custOrder = _context.CustOrder.Where(i => i.Id == OrderId).FirstOrDefault();
                custOrder.Status = "Active";
                _context.Update(custOrder);
                await _context.SaveChangesAsync();

            }
            catch
            {
                //doesnt work... my attempt on trying to throw a js error
                return RedirectToAction("Error", "Home");
            }

       
            return RedirectToAction("AllPendingOrders", "CustOrders");
        }


        [HttpPost, ActionName("CompleteOrder")]
        [Route("/CustOrder/CompleteOrder/:OrderId", Name = "CompleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteOrder(Guid OrderId)
        {

            try
            {
                CustOrder custOrder = _context.CustOrder.Where(i => i.Id == OrderId).FirstOrDefault();
                custOrder.Status = "Complete";
                _context.Update(custOrder);
                List<CustOrderItem> orderItemList = await _context.CustOrderItem.Where(i => i.CustOrderId == OrderId).Include(i => i.Item).ToListAsync();
                foreach(CustOrderItem orderItem in orderItemList)
                {
                    orderItem.Item.ItemStatus = ItemStatus.InCloset;
                    _context.Update(orderItem.Item);
                }
                await _context.SaveChangesAsync();

            }
            catch
            {
                //doesnt work... my attempt on trying to throw a js error
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("ActiveOrders", "CustOrders");
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
            Order_VM order = new Order_VM();
			order.Order = custOrder;
			order.CustOrderItemList = await _context.CustOrderItem.Where(i => i.CustOrderId == custOrder.Id).Include(i => i.Item).ThenInclude(i => i.ItemType).ToListAsync();


			return View(order);
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
                if (custOrder.Status == "Active" ||custOrder.Status == "Pending")
                {
                    List<CustOrderItem> orderItemList = await _context.CustOrderItem.Where(i => i.CustOrderId == id).Include(i => i.Item).ToListAsync();
                    foreach (CustOrderItem orderItem in orderItemList)
                    {
                        orderItem.Item.ItemStatus = ItemStatus.InCloset;
                        _context.Update(orderItem.Item);
                    }
                    await _context.SaveChangesAsync();
                }
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
