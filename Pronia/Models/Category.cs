using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Pronia.Models
{
    public class Category:BaseEntity
    {

        [MinLength(2),MaxLength(20)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}
