using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crimson_closet.Data;
using crimson_closet.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace crimson_closet.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public ItemsController(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<IActionResult> GetItemPhoto(Guid id)
        {
            var item = await _dbcontext.Item.FirstOrDefaultAsync(m => m.ItemId == id);
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
            var applicationDbContext = _dbcontext.Item.Include(i => i.ItemType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _dbcontext.Item == null)
            {
                return NotFound();
            }

            var item = await _dbcontext.Item
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
            ViewData["ItemTypeID"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemTypeID");
            ViewData["ItemDescription"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemDescription");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemCode,ItemStatus,ItemBrand,ItemSize,ItemColor,ItemTypeID,ItemGender")] Item item, IFormFile ItemPhoto)
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
                _dbcontext.Add(item);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemTypeID"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemTypeID", item.ItemTypeID);
            ViewData["ItemDescription"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemDescription");
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _dbcontext.Item == null)
            {
                return NotFound();
            }

            var item = await _dbcontext.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemTypeID"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemTypeID", item.ItemTypeID);
            ViewData["ItemDescription"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemDescription");

            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ItemId,ItemCode,ItemStatus,ItemBrand,ItemSize,ItemColor,ItemTypeID,ItemGender")] Item item, IFormFile ItemPhoto)
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

                    _dbcontext.Update(item);
                    await _dbcontext.SaveChangesAsync();
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
            ViewData["ItemTypeID"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemTypeID", item.ItemTypeID);
            ViewData["ItemDescription"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemDescription");
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _dbcontext.Item == null)
            {
                return NotFound();
            }

            var item = await _dbcontext.Item
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
            if (_dbcontext.Item == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Item'  is null.");
            }
            var item = await _dbcontext.Item.FindAsync(id);
            if (item != null)
            {
                _dbcontext.Item.Remove(item);
            }

            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(Guid id)
        {
            return _dbcontext.Item.Any(e => e.ItemId == id);
        }
    }
}
