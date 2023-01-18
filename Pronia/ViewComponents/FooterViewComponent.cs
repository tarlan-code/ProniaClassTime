using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;

namespace Pronia.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {

        readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
            return View(settings);
        }
    }
}
