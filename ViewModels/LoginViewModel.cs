using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email Address")]
		[Required(ErrorMessage = "Email address is required")]
		public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}