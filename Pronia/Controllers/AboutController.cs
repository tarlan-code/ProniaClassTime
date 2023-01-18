using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
