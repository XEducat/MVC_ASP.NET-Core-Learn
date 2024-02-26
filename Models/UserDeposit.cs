﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET_Core_Learn.Models
{
    public class UserDeposit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Зв`язок с користувачем
        public string UserId { get; set; }
        public AppUser User { get; set; }

        // Зв`язок с депозитом
        public int DepositId { get; set; }
        public Deposit Deposit { get; set; }

        // Інформація про депозит користувача
        public string Title { get; set; } // Назва депозита
        public decimal Amount { get; set; } // Сума депозита
        public int? SelectedTerm { get; set; } // Термін депозиту
        public double? InterestRate { get; set; } // Вибрана процентна ставка
        public bool IsEarlyClosureAllowed { get; set; } // Ставка може раніше закриватися чи ні

        public UserDeposit() { }

        public UserDeposit(Deposit deposit)
        {
            Deposit = deposit;
            DepositId = deposit.Id;
            Title = deposit.Title;
        }
    }
}