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
    [Authorize(Roles ="Admin,Superadmin")]
    public class BannersController : Controller
	{
        static List<int> orders = new List<int>() { 1, 2, 3, 4 };
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public BannersController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }



        public IActionResult Index()
        {
            return View(_context.Banners.ToList());
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Banner banner = _context.Banners.Find(id);
            if (banner == null) return NotFound();
            _context.Banners.Remove(banner);
            _context.SaveChanges();
            banner.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "banner"));
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Create()
        {
            if (_context.Banners.ToList().Count >= 4) return BadRequest();
            ViewBag.Orders = orders.Except(_context.Banners.Select(b => b.Order).ToList());

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBannerVM cb)
        {

        
            List<int> OpenOrders = orders.Except(_context.Banners.Select(b => b.Order).ToList()).ToList();
            ViewBag.Orders = OpenOrders;

            if (!OpenOrders.Any(o => o == cb.Order))
            {
                ModelState.AddModelError("Order", "There is no such order");
                return View();
            }

            if (cb.File is null && cb.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }
            if (cb.File is not null && cb.FileURL is not null)
            {
                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }

            string filename = null;

            if (cb.File is not null)
            {
                IFormFile file = cb.File;
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

                //string filename = Guid.NewGuid().ToString()+(file.FileName.Length>32 ? file.FileName.Substring(0,32) : file.FileName);
                filename = Guid.NewGuid().ToString() + file.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "banner", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            else
            {
                filename = cb.FileURL;
            }


            if (!ModelState.IsValid)return View();


            


            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<CreateBannerVM, Banner>()
            );

            var mapper = new Mapper(config);
            var banner = mapper.Map<Banner>(cb);
            banner.Image = filename;
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            Banner banner = _context.Banners.Find(Id);
            if (banner is null) return NotFound();

            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<Banner, CreateBannerVM>()
            );

            var mapper = new Mapper(config);
            var bn = mapper.Map<CreateBannerVM>(banner);
            if (banner.Image.StartsWith("http")) bn.FileURL = banner.Image;
            ViewBag.Orders = orders;
            return View(bn);
        }

        [HttpPost]
        public IActionResult Update(int? Id, CreateBannerVM cb)
        {
            if (Id is null || Id <= 0) return BadRequest();

            ViewBag.Orders = orders;

            if (!orders.Any(o => o == cb.Order))
            {
                ModelState.AddModelError("Order", "There is no such order");
                return View();
            }

            if (cb.File is null && cb.FileURL is null)
            {
                ModelState.AddModelError("File", "A image or image url must be definitely");
                return View();
            }

            if (cb.File is not null && cb.FileURL is not null)
            {

                ModelState.AddModelError("File", "Only a picture may be to be");
                return View();
            }

            string filename = null;

            if (cb.File is not null)
            {
                IFormFile file = cb.File;
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
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "banner", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            else
            {
                filename = cb.FileURL;
            }

            if (!ModelState.IsValid) return View();

            Banner exist = _context.Banners.Find(Id);
            if (exist is null) return NotFound();


            

            Banner OrderExist = _context.Banners.FirstOrDefault(o => o.Order == cb.Order);

            if (OrderExist != null)
            {
                OrderExist.Order = exist.Order;
            }

            exist.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "banner"));


            exist.Collection = cb.Collection;
            exist.Title = cb.Title;
            exist.BtnText = cb.BtnText;
            exist.BtnUrl = cb.BtnUrl;
            exist.Order = cb.Order;
            exist.Image = filename;



            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
