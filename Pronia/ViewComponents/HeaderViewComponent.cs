using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.ViewModels;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM basket = new HeaderVM()
            {
                Basket = GetBasket(),
                Settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value)
            };
            return View(basket);
        }


        BasketVM GetBasket()
        {
            BasketVM basket = new BasketVM();

            List<BasketItemVM> items = new List<BasketItemVM>();

            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }
            if(items is not null)
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
