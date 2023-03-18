using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class ShippingArea:BaseEntity
    {
        public string Icon { get; set; }

        [MinLength(1), MaxLength(30)]
        public string Title { get; set; }

        [MinLength(1), MaxLength(100)]
        public string ShortDesc { get; set; }
    }
}
