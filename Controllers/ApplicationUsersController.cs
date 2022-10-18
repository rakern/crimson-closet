using crimson_closet.Areas.Identity.Data;
using crimson_closet.Data;
using crimson_closet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace crimson_closet.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManger;


        public ApplicationUsersController(ILogger<HomeController> logger,
         UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager,
         ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _dbcontext = applicationDbContext;
            _userManager = userManager;
            _roleManger = roleManager;

        }

        public IActionResult Index()
        {
            //List<CrimsonClosetUserForDisplay> userList = new List<CrimsonClosetUserForDisplay>();

            //// Becuase we can't use the BasicUser that Ientity has created, we created a new User Model that will be used for displaying the users
            //foreach (var aspNetUser in _userManager.Users.ToList())
            //{
            //    userList.Add(new CrimsonClosetUserForDisplay() { 
            //        Id = new Guid(aspNetUser.Id), 
            //        FirstName = aspNetUser.FirstName,
            //        LastName = aspNetUser.LastName,
            //        Email = aspNetUser.Email,
            //        Username = aspNetUser.UserName
                    
            //    });
            //}

            return View(_userManager.Users.ToList());
        }

     

    }
}
