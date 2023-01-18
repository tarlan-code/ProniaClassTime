using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ShopVM shopVM = new ShopVM()
            {
                Categories = _context.Categories,
                Colors = _context.Colors,
            };
            return View(shopVM);
        }
    }
}
