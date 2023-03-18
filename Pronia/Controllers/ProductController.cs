using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ProductController : Controller
    {

        [HttpPost]
        public IActionResult AddBasket(int? id)
        {
            if (id is null || id < 0) return BadRequest();
            List<BasketItemVM> items = new List<BasketItemVM>();

            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }

            BasketItemVM itemVM = items.FirstOrDefault(p => p.Id == id);

            if(itemVM is null)
            {
                BasketItemVM item = new BasketItemVM
                {
                    Id = (int)id,
                    Count = 1
                };
                items.Add(item);
            }
            else
            {
                itemVM.Count++;
            }

            string value = JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", value, new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(1)
            });
            return Content("Ok");
        }
    }
}
