using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
