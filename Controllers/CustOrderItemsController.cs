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
    public class CustOrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustOrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustOrderItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CustOrderItem.Include(c => c.CustOrder).Include(c => c.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CustOrderItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.CustOrderItem == null)
            {
                return NotFound();
            }

            var custOrderItem = await _context.CustOrderItem
                .Include(c => c.CustOrder)
                .Include(c => c.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custOrderItem == null)
            {
                return NotFound();
            }

            return View(custOrderItem);
        }

        // GET: CustOrderItems/Create
        public IActionResult Create()
        {
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id");
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId");
            return View();
        }

        // POST: CustOrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustOrderId,ItemId")] CustOrderItem custOrderItem)
        {
            if (ModelState.IsValid)
            {
                custOrderItem.Id = Guid.NewGuid();
                _context.Add(custOrderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id", custOrderItem.CustOrderId);
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", custOrderItem.ItemId);
            return View(custOrderItem);
        }

        // GET: CustOrderItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.CustOrderItem == null)
            {
                return NotFound();
            }

            var custOrderItem = await _context.CustOrderItem.FindAsync(id);
            if (custOrderItem == null)
            {
                return NotFound();
            }
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id", custOrderItem.CustOrderId);
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", custOrderItem.ItemId);
            return View(custOrderItem);
        }

        // POST: CustOrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CustOrderId,ItemId")] CustOrderItem custOrderItem)
        {
            if (id != custOrderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custOrderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustOrderItemExists(custOrderItem.Id))
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
            ViewData["CustOrderId"] = new SelectList(_context.CustOrder, "Id", "Id", custOrderItem.CustOrderId);
            ViewData["ItemId"] = new SelectList(_context.Item, "ItemId", "ItemId", custOrderItem.ItemId);
            return View(custOrderItem);
        }

        // GET: CustOrderItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CustOrderItem == null)
            {
                return NotFound();
            }

            var custOrderItem = await _context.CustOrderItem
                .Include(c => c.CustOrder)
                .Include(c => c.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custOrderItem == null)
            {
                return NotFound();
            }

            return View(custOrderItem);
        }

        // POST: CustOrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.CustOrderItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CustOrderItem'  is null.");
            }
            var custOrderItem = await _context.CustOrderItem.FindAsync(id);
            if (custOrderItem != null)
            {
                _context.CustOrderItem.Remove(custOrderItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustOrderItemExists(Guid id)
        {
          return _context.CustOrderItem.Any(e => e.Id == id);
        }
    }
}
