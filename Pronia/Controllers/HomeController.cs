using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Abstractions.Services;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Services;
using Pronia.Utilies.Enums;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        readonly IEmailService _emailService;
        public HomeController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {

            IQueryable<Product> featuredproducts = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages).Take(4).AsQueryable();
            IQueryable<Product> latestproducts = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages).OrderByDescending(o => o.Date).Take(8).AsQueryable();

            HomeVM homeVM = new HomeVM
            {
                MainSliders = _context.MainSliders,
                ShippingAreas = _context.ShippingAreas.ToList(),
                TestimonialArea = _context.TestimonialAreas.FirstOrDefault(),
                Testimonials = _context.Testimonials,
                Brands = _context.Brands,
                Banners = _context.Banners.OrderBy(o => o.Order),
                FeaturedProducts = featuredproducts,
                LatestProducts = latestproducts
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult LoadProducts(int skip, int take)
        {


            var products = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages).Skip(skip).Take(take);


            return PartialView("_ProductPartial", products);
        }

        [HttpPost]
        public IActionResult QuickView(int? Id)
        {
            return ViewComponent("Modal", Id);
        }


        [HttpPost]
        public IActionResult LoadBasket()
        {

            return PartialView("_BasketPartial", GetBasket());
        }


        //E-mail Send
        public IActionResult SendMail()
        {
            _emailService.Send("heyderovterlan8@gmail.com", "asp.net Pronia Project", "Bu email Pronia mvc layihesi uchun bir emaildir");
            return RedirectToAction("Index");
        }



        BasketVM GetBasket()
        {
            BasketVM basket = new BasketVM();

            List<BasketItemVM> items = new List<BasketItemVM>();

            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }
            if (items is not null)
            {
                basket.Plants = new List<PlantBasketItemVM>();
                foreach (BasketItemVM item in items)
                {
                    PlantBasketItemVM plant = new PlantBasketItemVM();

                    plant.Product = _context.Products.Include(p => p.ProductImages).Where(p => p.ProductImages.Any(pi => pi.IsCover == true)).FirstOrDefault(p => p.Id == item.Id);
                    plant.Count = item.Count;
                    basket.Plants.Add(plant);
                    basket.TotalPrice += plant.Product.SellPrice * (100 - plant.Product.Discount) / 100 * item.Count;
                }
            }
            return basket;


        }


    }
}