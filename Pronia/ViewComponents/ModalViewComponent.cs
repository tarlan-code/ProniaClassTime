using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;

namespace Pronia.ViewComponents
{
    [ViewComponent(Name = "Modal")]
    public class ModalViewComponent:ViewComponent
    {
        readonly AppDbContext _context;

        public ModalViewComponent(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            
            var product = _context.Products.Include(p => p.ProductColors).ThenInclude(pc => pc.Color).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(p => p.ProductImages).FirstOrDefault(p => p.Id == Id);

            return View(product);
        }
    }
}
