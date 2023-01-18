using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class BlogListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
