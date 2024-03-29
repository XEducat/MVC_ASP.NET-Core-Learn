using MVC_ASP.NET_Core_Learn.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.ViewModels
{
    public class UserDepositViewModel
    {
        [Display(Name = "Назва депозиту")]
        public string Title { get; set; }

        [Display(Name = "Сума депозиту")]

        [DataType(DataType.Currency)]
        [Range(2, 200000, ErrorMessage = "Сума має бути від 2 до 200000")]
        public decimal Amount { get; set; } 

        [Display(Name = "Обраний термін")]
        [Required(ErrorMessage = "Ви не обрали термін депозиту")]
        public int SelectedTerm { get; set; }

        [Display(Name = "Процентна ставка")]
        [Required(ErrorMessage = "Ви не обрали процентну ставку")]
        public double? InterestRate { get; set; }

        [Display(Name = "Дозвіл на дострокове закриття")]
        public bool IsEarlyClosureAllowed { get; set; }

        public int DepositId { get; set; }
        public DepositTemplate Deposit { get; set; }
    }
}
