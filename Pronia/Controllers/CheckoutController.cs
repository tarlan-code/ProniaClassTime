using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
