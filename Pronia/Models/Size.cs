using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Size:BaseEntity
    {
        [MaxLength(20),MinLength(1)]
        public string Name { get; set; }

        public ICollection<ProductSize>? ProductSizes { get; set; }
    }
}
