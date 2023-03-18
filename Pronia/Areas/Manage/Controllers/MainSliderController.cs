using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilies.Extensions;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "Admin,Superadmin")]

    public class MainSliderController : Controller
    {

        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public MainSliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

      

        public IActionResult Index()
        {
            return View(_context.MainSliders);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            MainSlider ms = _context.MainSliders.Find(id);
            if (ms == null) return NotFound();
            _context.MainSliders.Remove(ms);
            _context.SaveChanges();
            ms.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "slider"));
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSliderVM cs)
        {
            
            if(cs.File is null && cs.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }
            if(cs.FileURL is not null && cs.File is not null)
            {
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }
            string filename = null;
            if (cs.File is not null)
            {
                IFormFile file = cs.File;
                if (!file.CheckType("image/"))
                {
                    ModelState.AddModelError("File", "File is not image");
                    return View();
                }
                if (file.Length > 200 * 1024)
                {
                    ModelState.AddModelError("File", "The size of the picture can not be large from 200 KB");
                    return View();
                }
                filename = Guid.NewGuid().ToString() + file.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "slider", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            else
            {
                filename = cs.FileURL;
            }
            if (!ModelState.IsValid) return View();
            

            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<CreateSliderVM, MainSlider>()
            );

            var mapper = new Mapper(config);
            var ms = mapper.Map<MainSlider>(cs);
            ms.Image = filename;
            _context.MainSliders.Add(ms);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            MainSlider ms = _context.MainSliders.Find(Id);
            if (ms is null) return NotFound();

            var config = new MapperConfiguration(cfg =>
           cfg.CreateMap<MainSlider, CreateSliderVM>()
           );

            var mapper = new Mapper(config);
            var cs = mapper.Map<CreateSliderVM>(ms);

            if (ms.Image.StartsWith("http"))cs.FileURL = ms.Image;

            return View(cs);
        }

        [HttpPost]
        public IActionResult Update(int? Id, CreateSliderVM cs)
        {
            if (Id is null || Id <= 0) return BadRequest();

            if (cs.File is null && cs.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }
            if (cs.FileURL is not null && cs.File is not null)
            {
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }

            string filename = null;
            if (cs.File is not null)
            {
                IFormFile file = cs.File;
                if (!file.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("File", "File is not image");
                    return View();
                }
                if (file.Length > 200 * 1024)
                {
                    ModelState.AddModelError("File", "The size of the picture can not be large from 200 KB");
                    return View();
                }
                filename = Guid.NewGuid().ToString() + file.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "slider", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            else
            {
                filename = cs.FileURL;
            }
            if (!ModelState.IsValid) return View();
            MainSlider exist = _context.MainSliders.Find(Id);
            if (exist is null) return NotFound();

            

            exist.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "slider"));



            exist.Offer = cs.Offer;
            exist.Title = cs.Title;
            exist.ShortDesc = cs.ShortDesc;
            exist.Image = filename;
            exist.BtnText = cs.BtnText;
      
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
