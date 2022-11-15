using crimson_closet.Data;
using Microsoft.AspNetCore.Mvc;
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
            return View(await applicationDbContext.ToListAsync());
        }
    }
}
