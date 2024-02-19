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
        public InterestRate InterestRate { get; set; }

        [Display(Name = "Терміни депозиту")]
        public IEnumerable<DepositTerm> Term { get; set; }   // Термін депозиту, [не доступний для зміни адміном наразі]

        [Display(Name = "Ставка без дострокового закриття(Вводити числа, виключно через символ '.')")]
        public double? InterestRateNoEarlyClosure { get; set; }

        [Display(Name = "Ставка з достроковим закриттям(Вводити числа, виключно через символ '.')")]
        public double? InterestRateEarlyClosure { get; set; }   
	}
}
