using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;

namespace Pronia.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        UserManager<AppUser> _userManager { get; }

        public ChatController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            
            return View(_userManager.Users);
        }
    }
}
