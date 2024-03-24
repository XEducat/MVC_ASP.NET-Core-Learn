using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "І'мя")]
        [Required(ErrorMessage = "Не вказане іи'я")]
        public string UserName { get; set; }

        [Display(Name = "Пошта")]
        [Required(ErrorMessage = "Не вказана електронна пошта")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не вказаний пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Підтвердження паролю")]
        [Required(ErrorMessage = "Потрібно підтвердити пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Країна")]
        [Required(ErrorMessage = "Не вказана країна")]
        public string State { get; set; }

        [Display(Name = "Місто")]
        [Required(ErrorMessage = "Не вказане місто")]
        public string City { get; set; }

        [Display(Name = "Вулиця")]
        [Required(ErrorMessage = "Не вказана вулиця")]
        public string Street { get; set; }
    }
}