using Pronia.Models;

namespace Pronia.ViewModels
{
    public class ShopVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Color> Colors { get; set; }
    }
}
