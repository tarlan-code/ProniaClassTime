using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Superadmin")]

    public class TestimonialAreaController : Controller
    {
        readonly AppDbContext _context;

        public TestimonialAreaController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.Testimonials = _context.Testimonials.ToList();
            return View(_context.TestimonialAreas.FirstOrDefault());
        }


        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            TestimonialArea ta = _context.TestimonialAreas.Find(id);
            if (ta == null) return NotFound();
            _context.TestimonialAreas.Remove(ta);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Create()
        {
            if (_context.TestimonialAreas.ToList().Count >= 1)
            {
                return BadRequest();
            }
            return View();
        }


        [HttpPost]
        public ActionResult Create(TestimonialArea ta)
        {
            if (!ModelState.IsValid) return View();
            _context.TestimonialAreas.Add(ta);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            TestimonialArea ta = _context.TestimonialAreas.Find(Id);
            if (ta is null) return NotFound();


            return View(ta);
        }

        [HttpPost]
        public IActionResult Update(int? Id, TestimonialArea ta)
        {
            if (Id is null || Id <= 0 || Id != ta.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            TestimonialArea exist = _context.TestimonialAreas.Find(Id);
            if (exist is null) return NotFound();


            exist.Title = ta.Title;
            exist.Desc = ta.Desc;



            _context.TestimonialAreas.Update(exist);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
