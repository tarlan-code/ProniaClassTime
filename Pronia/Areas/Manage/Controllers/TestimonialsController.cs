using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Superadmin")]
    public class TestimonialsController : Controller
    {
        readonly AppDbContext _context;

        public TestimonialsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TestimonialAreas = _context.TestimonialAreas.FirstOrDefault();
            return View(_context.Testimonials.ToList());
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Testimonial tm = _context.Testimonials.Find(id);
            if (tm == null) return NotFound();
            _context.Testimonials.Remove(tm);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Testimonial tm)
        {

            if (!ModelState.IsValid) return View();
            _context.Testimonials.Add(tm);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            Testimonial tm = _context.Testimonials.Find(Id);
            if (tm is null) return NotFound();


            return View(tm);
        }

        [HttpPost]
        public IActionResult Update(int? Id, Testimonial tm)
        {
            if (Id is null || Id <= 0 || Id != tm.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            Testimonial exist = _context.Testimonials.Find(Id);
            if (exist is null) return NotFound();

            exist.Name = tm.Name;
            exist.Surname = tm.Surname;
            exist.Image = tm.Image;
            exist.Occupation = tm.Occupation;
            exist.Comment = tm.Comment;


            _context.Testimonials.Update(exist);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
