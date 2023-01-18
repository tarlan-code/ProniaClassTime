using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class SingleProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
