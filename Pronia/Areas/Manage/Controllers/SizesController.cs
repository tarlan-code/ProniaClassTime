using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "Admin,Superadmin")]

    public class SizesController : Controller
	{
        readonly AppDbContext _context;

        public SizesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Sizes);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Size size = _context.Sizes.Find(id);
            if (size == null) return NotFound();
            _context.Sizes.Remove(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Size size)
        {

            if (!ModelState.IsValid) return View();
            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            Size size = _context.Sizes.Find(Id);
            if (size is null) return NotFound();


            return View(size);
        }

        [HttpPost]
        public IActionResult Update(int? Id, Size size)
        {
            if (Id is null || Id <= 0 || Id != size.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            Size exist = _context.Sizes.Find(Id);
            if (exist is null) return NotFound();

            exist.Name = size.Name;

            _context.Sizes.Update(exist);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
