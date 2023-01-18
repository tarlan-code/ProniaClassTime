using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class LoginRegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
