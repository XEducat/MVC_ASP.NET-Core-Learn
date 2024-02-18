using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Display(Name = "Email addres")]
        [Required(ErrorMessage = "Email addres is required")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confrirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Display(Name = "Street")]
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }
    }
}