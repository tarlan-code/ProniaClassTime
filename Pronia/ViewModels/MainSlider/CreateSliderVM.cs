using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
	public class CreateSliderVM
	{
        [Range(1, 100)]
        public int Offer { get; set; }
        [MinLength(1), MaxLength(30)]
        public string Title { get; set; }
        [MinLength(1), MaxLength(100)]
        public string ShortDesc { get; set; }
        [MinLength(1), MaxLength(20)]
        public string BtnText { get; set; }
        public IFormFile? File { get; set; }
        [Url]
        public string? FileURL { get; set; }
    }
}
