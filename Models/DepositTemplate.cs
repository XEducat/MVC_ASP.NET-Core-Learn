using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MVC_ASP.NET_Core_Learn.Data.Enums;


namespace MVC_ASP.NET_Core_Learn.Models
{
    public class DepositTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }                          // Айді
        public string Title { get; set; }                    // Назва депозиту
        public string ShortDescription { get; set; }         // Короткий опис депозиту
        public bool Replenishment { get; set; }              // Поповнення
        public InterestPayment InterestPayment { get; set; }       // Виплата процентів
        public List<DepositTerm> Terms { get; set; }   // Термін депозиту
        public double? InterestRateNoEarlyClosure { get; set; } // Ставка без дострокового закриття
        public double? InterestRateEarlyClosure { get; set; }   // Ставка з достроковим закриттям
	}
}
