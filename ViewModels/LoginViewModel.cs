﻿using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Пошта")]
		[EmailAddress]
        [Required(ErrorMessage = "Поле обов'язкове")]
        public string EmailAddress { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обов'язкове")]
        public string Password { get; set; }
    }
}