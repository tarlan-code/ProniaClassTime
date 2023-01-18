using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "Admin,Superadmin")]

    public class ColorsController : Controller
    {
        readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Colors.ToList());
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Color col = _context.Colors.Find(id);
            if (col == null) return NotFound();
            _context.Colors.Remove(col);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color col)
        {

            if (!ModelState.IsValid) return View();
            col.IsActive = true;
            _context.Colors.Add(col);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            Color col = _context.Colors.Find(Id);
            if (col is null) return NotFound();


            return View(col);
        }

        [HttpPost]
        public IActionResult Update(int? Id, Color col)
        {
            if (Id is null || Id <= 0 || Id != col.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            Color exist = _context.Colors.Find(Id);
            if (exist is null) return NotFound();

            exist.Name = col.Name;

            _context.Colors.Update(exist);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeStatus(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Color col = _context.Colors.Find(id);
            if (col == null) return NotFound();

            if (col.IsActive) col.IsActive = false;
            else col.IsActive = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
