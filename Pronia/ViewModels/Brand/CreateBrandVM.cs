using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class CreateBrandVM
    {
        public IFormFile? File { get; set; }

        [Url]
        public string? FileURL { get; set; }
        public string Link { get; set; }
    }
}
