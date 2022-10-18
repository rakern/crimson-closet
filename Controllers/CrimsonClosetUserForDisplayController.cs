using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crimson_closet.Controllers
{
    public class CrimsonClosetUserForDisplayController : Controller
    {
        private readonly UserManager<BasicUser> _userManager;
       

        public CrimsonClosetUserForDisplayController(UserManager<BasicUser> userManager)
        {
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
            List<CrimsonClosetUserForDisplay> userList = new List<CrimsonClosetUserForDisplay>();

            // Becuase we can't use the BasicUser that Ientity has created, we created a new User Model that will be used for displaying the users
            foreach (var aspNetUser in _userManager.Users.ToList())
            {
                userList.Add(new CrimsonClosetUserForDisplay() { 
                    Id = new Guid(aspNetUser.Id), 
                    FirstName = aspNetUser.FirstName,
                    LastName = aspNetUser.LastName,
                    Email = aspNetUser.Email,
                    Username = aspNetUser.UserName
                    
                });
            }
            return View(userList);
        }

     

    }
}
