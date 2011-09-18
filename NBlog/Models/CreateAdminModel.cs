using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NBlog.Models
{
    public class CreateAdminModel
    {
        [Required(ErrorMessage = "User name required")]
        [DisplayName("Admin user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Name required")]
        [DisplayName("Admin name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password required")]
        [DisplayName("Admin password")]
        [MinLength(6, ErrorMessage = "Minimum length is 6")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Password confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string PasswordConfirmation { get; set; }
    }
}