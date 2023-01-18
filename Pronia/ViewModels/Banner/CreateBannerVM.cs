using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
	public class CreateBannerVM
	{
        [MinLength(1), MaxLength(30)]
        public string Collection { get; set; }
        [MinLength(1), MaxLength(30)]
        public string Title { get; set; }
        [MinLength(1), MaxLength(20)]
        public string BtnText { get; set; }
        public string BtnUrl { get; set; }
        public IFormFile? File { get; set; }

        [Url]
        public string? FileURL { get; set; }
        [Range(1,4)]
        public int Order { get; set; }

    }
}
