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
    public class OrderItemizedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemizedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderItemizeds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderItemized.Include(o => o.CustOrder).Include(o => o.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderItemizeds/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.OrderItemized == null)
            {
                return NotFound();
            }

            var orderItemized = await _context.OrderItemized
                .Include(o => o.CustOrder)
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItemized == null)
            {
                return NotFound();
            }

            return View(orderItemized);
        }

        // GET: OrderItemizeds/Create
        public IActionResult Create()
        {
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id");
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId");
            return View();
        }

        // POST: OrderItemizeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,CustOrderId")] OrderItemized orderItemized)
        {
            if (ModelState.IsValid)
            {
                orderItemized.Id = Guid.NewGuid();
                _context.Add(orderItemized);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id", orderItemized.CustOrderId);
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", orderItemized.ItemId);
            return View(orderItemized);
        }

        // GET: OrderItemizeds/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.OrderItemized == null)
            {
                return NotFound();
            }

            var orderItemized = await _context.OrderItemized.FindAsync(id);
            if (orderItemized == null)
            {
                return NotFound();
            }
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id", orderItemized.CustOrderId);
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", orderItemized.ItemId);
            return View(orderItemized);
        }

        // POST: OrderItemizeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ItemId,CustOrderId")] OrderItemized orderItemized)
        {
            if (id != orderItemized.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItemized);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemizedExists(orderItemized.Id))
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
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id", orderItemized.CustOrderId);
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", orderItemized.ItemId);
            return View(orderItemized);
        }

        // GET: OrderItemizeds/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.OrderItemized == null)
            {
                return NotFound();
            }

            var orderItemized = await _context.OrderItemized
                .Include(o => o.CustOrder)
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItemized == null)
            {
                return NotFound();
            }

            return View(orderItemized);
        }

        // POST: OrderItemizeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.OrderItemized == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderItemized'  is null.");
            }
            var orderItemized = await _context.OrderItemized.FindAsync(id);
            if (orderItemized != null)
            {
                _context.OrderItemized.Remove(orderItemized);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemizedExists(Guid id)
        {
          return _context.OrderItemized.Any(e => e.Id == id);
        }
    }
}
