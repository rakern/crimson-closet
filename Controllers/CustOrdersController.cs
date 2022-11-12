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
    public class CustOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CustOrder.Include(c => c.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CustOrders/Details/5
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
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,QuantOfItems,CheckOutDate,ReturnByDate")] CustOrder custOrder)
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ApplicationUserId,QuantOfItems,CheckOutDate,ReturnByDate")] CustOrder custOrder)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", custOrder.ApplicationUserId);
            return View(custOrder);
        }

        // GET: CustOrders/Delete/5
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
            return RedirectToAction(nameof(Index));
        }

        private bool CustOrderExists(Guid id)
        {
          return _context.CustOrder.Any(e => e.Id == id);
        }
    }
}
