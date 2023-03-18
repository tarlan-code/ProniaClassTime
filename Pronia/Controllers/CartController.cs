using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
