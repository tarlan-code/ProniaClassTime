using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class UserLoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPresistance { get; set; }
    }
}
