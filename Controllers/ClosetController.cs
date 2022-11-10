using Microsoft.AspNetCore.Mvc;

namespace crimson_closet.Controllers
{
    public class ClosetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
