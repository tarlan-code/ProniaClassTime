using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
	public class ShippingAreaVM
	{
       
        public IFormFile? File { get; set; }

        [Url]
        public string? FileURL { get; set; }

        [MinLength(1), MaxLength(30)]
        public string Title { get; set; }

        [MinLength(1), MaxLength(100)]
        public string ShortDesc { get; set; }
    }
}
