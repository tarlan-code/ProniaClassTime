using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class CreateProductVM
    {
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }
        [MaxLength(1000), MinLength(1)]
        public string Desc { get; set; }
        [Range(0.00, Double.MaxValue)]
        public double CostPrice { get; set; }
        [Range(0.00, Double.MaxValue)]
        public double SellPrice { get; set; }
        [Range(0,100)]
        public int Discount { get; set; }

        public IFormFile CoverImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public ICollection<IFormFile>? OtherImages { get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}
