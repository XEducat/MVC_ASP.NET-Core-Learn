using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data
{
	public class Seed
	{
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate(); // Убедимся, что база данных создана

            if (context.Deposits.Any())
            {
                // База данных уже заполнена, поэтому не нужно ничего делать
                return;
            }

            // Начальные данные для депозитов
            var seedDeposits = new List<Deposit>
            {
                new Deposit
                {
                    Title = "Депозит 1",
                    ShortDescription = "Короткий опис депозиту 1",
                    Replenishment = true,
                    InterestRate = InterestRate.WhenReturning,
                    Term = new List<DepositTerm>
                    {
                        new DepositTerm { NumberMonths = 12 },
                        new DepositTerm { NumberMonths = 24 }
                    },
                    InterestRateNoEarlyClosure = 4.0,
                    InterestRateEarlyClosure = 4.5
                },
                new Deposit
                {
                    Title = "Депозит 2",
                    ShortDescription = "Короткий опис депозиту 2",
                    Replenishment = false,
                    InterestRate = InterestRate.Monthly,
                    Term = new List<DepositTerm>
                    {
                        new DepositTerm { NumberMonths = 6 },
                        new DepositTerm { NumberMonths = 12 }
                    },
                    InterestRateNoEarlyClosure = null,
                    InterestRateEarlyClosure = 5.0
                },
	            new Deposit
	            {
		            Title = "Депозит 3",
		            ShortDescription = "Короткий опис депозиту 3",
		            Replenishment = true,
		            InterestRate = InterestRate.Monthly,
		            Term = new List<DepositTerm>
		            {
			            new DepositTerm { NumberMonths = 3 },
			            new DepositTerm { NumberMonths = 6 }
		            },
		            InterestRateNoEarlyClosure = 3.5,
		            InterestRateEarlyClosure = 4.0
	            },
	            new Deposit
	            {
		            Title = "Депозит 4",
		            ShortDescription = "Короткий опис депозиту 4",
		            Replenishment = false,
		            InterestRate = InterestRate.WhenReturning,
		            Term = new List<DepositTerm>
		            {
			            new DepositTerm { NumberMonths = 6 },
			            new DepositTerm { NumberMonths = 12 },
			            new DepositTerm { NumberMonths = 24 }
		            },
		            InterestRateNoEarlyClosure = 3.0,
		            InterestRateEarlyClosure = 3.5
	            },
	            new Deposit
	            {
		            Title = "Депозит 5",
		            ShortDescription = "Короткий опис депозиту 5",
		            Replenishment = true,
		            InterestRate = InterestRate.Monthly,
		            Term = new List<DepositTerm>
		            {
			            new DepositTerm { NumberMonths = 6 },
			            new DepositTerm { NumberMonths = 12 },
			            new DepositTerm { NumberMonths = 18 }
		            },
		            InterestRateNoEarlyClosure = 4.0,
		            InterestRateEarlyClosure = 4.5
	            }
			};


            // Добавляем начальные данные в базу данных
            context.Deposits.AddRange(seedDeposits);
            context.SaveChanges();
        }


        //public void AddNewUser()
        //{
        //    // Создать нового покупателя
        //    User customer = new User
        //    {
        //        Mail = "Иван",
        //        Birdthday = DateTime.Now,
        //    };

        //    // Добавить в DbSet
        //    Users.Add(customer);

        //    // Сохранить изменения в базе данных
        //    SaveChanges();
        //}
    }
}
