using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace crimson_closet.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ItemTypesController : Controller
    {
        
        private readonly ApplicationDbContext _dbcontext;

        public ItemTypesController(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        // GET: ItemTypes
        public async Task<IActionResult> Index()
        {
            return View(await _dbcontext.ItemType.ToListAsync());
        }

        // GET: ItemTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _dbcontext.ItemType == null)
            {
                return NotFound();
            }

            var itemType = await _dbcontext.ItemType
                .FirstOrDefaultAsync(m => m.ItemTypeID == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // GET: ItemTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemTypeID,ItemDescription")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                itemType.ItemTypeID = Guid.NewGuid();
                _dbcontext.Add(itemType);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemType);
        }

        // GET: ItemTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _dbcontext.ItemType == null)
            {
                return NotFound();
            }

            var itemType = await _dbcontext.ItemType.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ItemTypeID,ItemDescription")] ItemType itemType)
        {
            if (id != itemType.ItemTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbcontext.Update(itemType);
                    await _dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.ItemTypeID))
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
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _dbcontext.ItemType == null)
            {
                return NotFound();
            }

            var itemType = await _dbcontext.ItemType
                .FirstOrDefaultAsync(m => m.ItemTypeID == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // POST: ItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_dbcontext.ItemType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ItemType'  is null.");
            }
            var itemType = await _dbcontext.ItemType.FindAsync(id);
            if (itemType != null)
            {
                _dbcontext.ItemType.Remove(itemType);
            }

            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemTypeExists(Guid id)
        {
            return _dbcontext.ItemType.Any(e => e.ItemTypeID == id);
        }
    }
}
