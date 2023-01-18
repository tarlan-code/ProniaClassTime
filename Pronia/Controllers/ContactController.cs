using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
