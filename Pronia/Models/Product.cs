using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Product:BaseEntity
    {

        [MaxLength(50),MinLength(1)]
        public string Name { get; set; }
        [MaxLength(1000), MinLength(1)]
        public string Desc { get; set; }
        [MinLength(1)]
        public string SKU { get; set; }
        [Range(0.00,Double.MaxValue)]
        public double CostPrice { get; set; }
        [Range(0.00, Double.MaxValue)]
        public double SellPrice { get; set; }
        [Range(1,100)]
        public int Discount { get; set; }
        public bool IsDeleted { get; set; } = true;
        public ICollection<ProductSize>? ProductSizes { get; set; }
        public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
        public ProductInfo? ProductInfo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
 


    }
}
