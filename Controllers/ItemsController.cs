using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;

namespace crimson_closet.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetItemPhoto(Guid id)
        {
            var item = await _context.Item.FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }
            var imageData = item.ItemPhoto;

            return File(imageData, "image/jpg");
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Item.Include(i => i.ItemType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["ItemTypeID"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemTypeID");
            ViewData["ItemDescription"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemDescription");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemCode,ItemStatus,ItemBrand,ItemSize,ItemColor,ItemTypeID")] Item item, IFormFile ItemPhoto)
        {
            if (ModelState.IsValid)
            {
                if (ItemPhoto != null && ItemPhoto.Length > 0)
                {
                    var memoryStream = new MemoryStream();
                    await ItemPhoto.CopyToAsync(memoryStream);
                    item.ItemPhoto = memoryStream.ToArray();
                }

                item.ItemId = Guid.NewGuid();
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemTypeID"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemTypeID", item.ItemTypeID);
            ViewData["ItemDescription"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemDescription");
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemTypeID"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemTypeID", item.ItemTypeID);
            ViewData["ItemDescription"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemDescription");

            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ItemId,ItemCode,ItemStatus,ItemBrand,ItemSize,ItemColor,ItemTypeID")] Item item, IFormFile ItemPhoto)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ItemPhoto != null && ItemPhoto.Length > 0)
                    {
                        var memoryStream = new MemoryStream();
                        await ItemPhoto.CopyToAsync(memoryStream);
                        item.ItemPhoto = memoryStream.ToArray();
                    }

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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
            ViewData["ItemTypeID"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemTypeID", item.ItemTypeID);
            ViewData["ItemDescription"] = new SelectList(_context.ItemType, "ItemTypeID", "ItemDescription");
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Item == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Item'  is null.");
            }
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(Guid id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
