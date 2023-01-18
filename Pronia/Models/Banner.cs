using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
	public class Banner:BaseEntity
	{
        [MinLength(1), MaxLength(30)]
        public string Collection { get; set; }
        [MinLength(1), MaxLength(30)]
        public string Title { get; set; }
        [MinLength(1), MaxLength(20)]
        public string BtnText { get; set; }
		public string BtnUrl { get; set; }
		public string Image { get; set; }
		public int Order { get; set; }

	}
}
