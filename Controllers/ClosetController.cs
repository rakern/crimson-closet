using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace crimson_closet.Controllers
{
	public class ClosetController : Controller
	{
		private readonly ApplicationDbContext _dbcontext;
		private readonly UserManager<ApplicationUser> _userManager;

		public ClosetController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_dbcontext = context;
			_userManager = userManager;
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

			//filters out all items except those incloset
			var inClosetItems = await applicationDbContext.Where(i => i.ItemStatus == ItemStatus.InCloset).ToListAsync();

			return View(inClosetItems);
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

		// POST: Closet/AddToCart/ItemId
		[HttpPost, ActionName("AddToCart")]
		[Route("/Closet/AddToCart/:ItemId", Name = "AddToCart")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddToCart(Guid ItemId)
		{
			if(!User.Identity.IsAuthenticated)
			{
				return Redirect("~/Identity/Account/Login");
			}
			try
			{
				ApplicationUser currentUser = await _userManager.GetUserAsync(User);
				CartItem cartItem= new CartItem();
				cartItem.Id = Guid.NewGuid();
				cartItem.ItemId = ItemId;
				cartItem.ApplicationUserId = currentUser.Id;
				_dbcontext.Add(cartItem);
				await _dbcontext.SaveChangesAsync();

			}
			catch
			{
				//doesnt work... my attempt on trying to throw a js error
				return Content("<script language='javascript' type='text/javascript'>alert('Could not Add to cart!');</script>");
			}

			await _dbcontext.SaveChangesAsync();
			return RedirectToAction("CustomerCart", "CartItem");
		}
	}
}