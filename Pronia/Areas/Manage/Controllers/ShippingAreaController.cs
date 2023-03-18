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

    public class ShippingAreaController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public ShippingAreaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.ShippingAreas.ToList());
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            ShippingArea sa = _context.ShippingAreas.Find(id);
            if (sa == null) return NotFound();
            _context.ShippingAreas.Remove(sa);
            _context.SaveChanges();
            sa.Icon.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "shipping"));

            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            if (_context.ShippingAreas.ToList().Count >= 3)
            {
                return BadRequest();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(ShippingAreaVM sa)
        {
            if (sa.File is null && sa.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }
            if (sa.File is not null && sa.FileURL is not null)
            {
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }

            string filename = null;

            if (sa.File is not null)
            {
                IFormFile file = sa.File;

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

                filename = Guid.NewGuid() + file.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "shipping", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            else
            {
                filename = sa.FileURL;
            }

            if (!ModelState.IsValid) return View();

            


            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<ShippingAreaVM, ShippingArea>()
            );

            var mapper = new Mapper(config);
            var ShipArea = mapper.Map<ShippingArea>(sa);
            ShipArea.Icon = filename;
            _context.ShippingAreas.Add(ShipArea);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            ShippingArea sa = _context.ShippingAreas.Find(Id);
            if (sa is null) return NotFound();

            var config = new MapperConfiguration(cfg =>
           cfg.CreateMap<ShippingArea, ShippingAreaVM>()
           );

            var mapper = new Mapper(config);
            var ShipArea = mapper.Map<ShippingAreaVM>(sa);
            if(sa.Icon.StartsWith("http")) ShipArea.FileURL = sa.Icon;
            return View(ShipArea);

        }

        [HttpPost]
        public IActionResult Update(int? Id, ShippingAreaVM sa)
        {
            if (Id is null || Id <= 0) return BadRequest();

            if (sa.File is null && sa.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }

            if (sa.File is not null && sa.FileURL is not null)
            {
                
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }

            string filename = null;
            if (sa.File is not null)
            {
                IFormFile file = sa.File;

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

                filename = Guid.NewGuid() + file.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "shipping", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            else
            {
                filename = sa.FileURL;
            }

            if (!ModelState.IsValid) return View();
            ShippingArea exist = _context.ShippingAreas.Find(Id);
            if (exist is null) return NotFound();

            

            exist.Icon.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "shipping"));


            exist.Icon = filename;
            exist.Title = sa.Title;
            exist.ShortDesc = sa.ShortDesc;


            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
