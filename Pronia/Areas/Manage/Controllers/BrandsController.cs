using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilies.Enums;
using Pronia.Utilies.Extensions;
using Pronia.ViewModels;
using System;


namespace Pronia.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "Admin,Superadmin")]
	public class BrandsController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

      

        public BrandsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Brands);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Brand brand = _context.Brands.Find(id);
            if (brand == null) return NotFound();
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            brand.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "brand"));

            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBrandVM bd)
        {
            if (bd.File is null && bd.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }
            if (bd.FileURL is not null && bd.File is not null)
            {
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }
            string filename = null;
            if (bd.File is not null)
            {
                IFormFile file = bd.File;
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
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "brand", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            else
            {
                filename = bd.FileURL;
            }

            if (!ModelState.IsValid) return View();


            


            Brand brand  = new Brand()
            {
                Link = bd.Link,
                Image = filename
            };

            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            Brand brand = _context.Brands.Find(Id);
            if (brand is null) return NotFound();
            

            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<Brand, CreateBrandVM>()
            );

            var mapper = new Mapper(config);
            var bd = mapper.Map<CreateBrandVM>(brand);

            if (brand.Image.StartsWith("http")) bd.FileURL = brand.Image;

            return View(bd);
        }

        [HttpPost]
        public IActionResult Update(int? Id, CreateBrandVM bd)
        {
            if (Id is null || Id <= 0) return BadRequest();

            if (bd.File is null && bd.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }
            if (bd.FileURL is not null && bd.File is not null)
            {
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }

            string filename = null;
            if (bd.File is not null)
            {
                IFormFile file = bd.File;
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
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "brand", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            else
            {
                filename = bd.FileURL;
            }

            if (!ModelState.IsValid) return View();
            Brand exist = _context.Brands.Find(Id);
            if (exist is null) return NotFound();


           

            exist.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "brand"));

            exist.Image = filename;
            exist.Link = bd.Link;

    
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        

        
    }
}
