using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
