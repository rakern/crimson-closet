using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace crimson_closet.Controllers
{
    public class ClosetController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public ClosetController(ApplicationDbContext context)
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

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _dbcontext.Item.Include(i => i.ItemType);

            ViewData["ItemTypeID"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemTypeID");
            ViewData["ItemTypes"] = new SelectList(_dbcontext.ItemType, "ItemTypeID", "ItemDescription");

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
    }
}
