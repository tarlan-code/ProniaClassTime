using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class FaqController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
