using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Testimonial:BaseEntity
    {

        [MinLength(2),MaxLength(20)]
        public string Name { get; set; }
        [MinLength(2),MaxLength(25)]
        public string Surname { get; set; }
        public string Image { get; set; }
        [MinLength(1), MaxLength(15)]
        public string Occupation { get; set; }

        [MinLength(10), MaxLength(300)]

        public string Comment { get; set; }
    }
}
