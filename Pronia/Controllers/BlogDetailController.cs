using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class BlogDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
