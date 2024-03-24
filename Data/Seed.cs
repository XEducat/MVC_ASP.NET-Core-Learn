using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data
{
    public class Seed
	{
		public static void Initialize(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
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
						Title = "Стандарт",
						ShortDescription = "класичний вклад на обраний строк з можливістю поповнення та щомісячними %",
						Replenishment = true,
						InterestPayment = InterestPayment.Monthly,
						Terms = new List<DepositTerm>
						{
							new DepositTerm { NumberMonths = 18 },
							new DepositTerm { NumberMonths = 24 }
						},
						InterestRateNoEarlyClosure = 13,
						InterestRateEarlyClosure = null
					},
					new Deposit
					{
						Title = "Джуніор",
						ShortDescription = "заощадження на майбутнє дитини",
						Replenishment = false,
						InterestPayment = InterestPayment.Monthly,
						Terms = new List<DepositTerm>
						{
							new DepositTerm { NumberMonths = 6 },
							new DepositTerm { NumberMonths = 12 }
						},
						InterestRateNoEarlyClosure = null,
						InterestRateEarlyClosure = 5.0
					},
					new Deposit
					{
						Title = "Слава Героям",
						ShortDescription = "спеціальний вклад для героїчних захисників і захисниць України",
						Replenishment = true,
						InterestPayment = InterestPayment.Monthly,
						Terms = new List<DepositTerm>
						{
							new DepositTerm { NumberMonths = 3 },
							new DepositTerm { NumberMonths = 6 }
						},
						InterestRateNoEarlyClosure = 3.5,
						InterestRateEarlyClosure = 4.0
					},
					new Deposit
					{
						Title = "Вільний",
						ShortDescription = "вільне поповнення та зняття коштів у будь-який день після дати відкриття вкладу",
						Replenishment = false,
						InterestPayment = InterestPayment.WhenReturning,
						Terms = new List<DepositTerm>
						{
							new DepositTerm { NumberMonths = 6 },
							new DepositTerm { NumberMonths = 12 },
							new DepositTerm { NumberMonths = 24 }
						},
						InterestRateNoEarlyClosure = null,
						InterestRateEarlyClosure = 2.5
					},
					new Deposit
					{
						Title = "Послуга накопичення «Скарбничка»",
						ShortDescription = "простий сервіс накопичень, зручні варіанти поповнення з картки",
						Replenishment = true,
						InterestPayment = InterestPayment.Monthly,
						Terms = new List<DepositTerm>
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
		}


		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.User))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

				//Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
				string adminUserEmail = "vadimSdeveloper@gmail.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new AppUser()
					{
						UserName = "vadimS",
						Email = adminUserEmail,
						EmailConfirmed = true,
						Address = new Address()
						{
							Street = "123 Main St",
							City = "Charlotte",
							State = "NC"
						}
					};
					await userManager.CreateAsync(newAdminUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}

				string appUserEmail = "user@etickets.com";

				var appUser = await userManager.FindByEmailAsync(appUserEmail);
				if (appUser == null)
				{
					var newAppUser = new AppUser()
					{
						UserName = "app-user",
						Email = appUserEmail,
						EmailConfirmed = true,
						Address = new Address()
						{
							Street = "123 Main St",
							City = "Charlotte",
							State = "NC"
						}
					};
					await userManager.CreateAsync(newAppUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
				}
			}
		}
	}
}
