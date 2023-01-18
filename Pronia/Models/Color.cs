using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Color:BaseEntity
    {
        [MinLength(2),MaxLength(20)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ProductColor>? ProductColors { get; set; }
    }
}
