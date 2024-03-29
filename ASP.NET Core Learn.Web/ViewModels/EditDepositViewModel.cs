using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.ViewModels
{
	public class EditDepositViewModel
	{
        public int Id { get; set; }
		[Display(Name = "Назва депозиту")]
		public string Title { get; set; }

		[Display(Name = "Короткий опис депозиту")]
		public string ShortDescription { get; set; }

		[Display(Name = "Поповнення можливе")]
		public bool Replenishment { get; set; }              

        [Display(Name = "Виплата процентів")]
        public InterestPayment InterestRate { get; set; }

        [Display(Name = "Терміни депозиту")]
        public List<DepositTerm> Terms { get; set; }   // Термін депозиту, [не доступний для зміни адміном наразі]

        [Display(Name = "Ставка без дострокового закриття(Вводити числа, через символ '.')")]
        public double? InterestRateNoEarlyClosure { get; set; }

        [Display(Name = "Ставка з достроковим закриттям(Вводити числа, через символ '.')")]
        public double? InterestRateEarlyClosure { get; set; }   
	}
}
