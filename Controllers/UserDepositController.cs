using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.Repository;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    /// <summary>
    ///  Контроллер управління депозитами користувачів.
    /// </summary>
    [Authorize]
    public class UserDepositController : Controller
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IUserDepositRepository _userDepositRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserDepositController(IDepositRepository depositRepository, UserManager<AppUser> userManager, IUserDepositRepository userDepositRepository)
        {
            _depositRepository = depositRepository;
            _userManager = userManager;
			_userDepositRepository = userDepositRepository;
        }

        [HttpGet("Open/{depositId}")]
        public async Task<IActionResult> OrderForm(int depositId)
        {
            var deposit = await _depositRepository.GetByIdAsync(depositId);
            if (deposit == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Errors = new List<string> { "Депозит не знайдено" }
                };
                return View("Error", errorViewModel);
            }

            UserDepositViewModel viewModel = new UserDepositViewModel
            {
                Deposit = deposit,
                DepositId = deposit.Id,
                Title = deposit.Title,
            };

            return View(viewModel);
        }

        [HttpPost("Open/{depositId}")]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> OrderForm(UserDepositViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(nameof(OrderForm), viewModel);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Error", "Home"); // Якщо користувача не знайдено

            var deposit = await _depositRepository.GetByIdAsync(viewModel.DepositId);
            if (deposit == null)
                return RedirectToAction("Error", "Home"); // Якщо депозита не знайдено

            // TODO: Зробити нормальний звязок термінів з депозитами,
            // для прив`язки UserDeposit до Deposit

            // Створюємо новий об'єкт UserDeposit з даними з viewModel
            var userDeposit = new UserDeposit(deposit)
            {
                User = currentUser,
                UserId = currentUser.Id,
                Amount = viewModel.Amount,
                InterestRate = viewModel.InterestRate,
                IsEarlyClosureAllowed = viewModel.IsEarlyClosureAllowed,
                SelectedTerm = viewModel.SelectedTerm,
				CreatedDate = DateTime.Now,
			};

			// Додаємо userDeposit до контексту даних і зберігаємо зміни
			_userDepositRepository.Add(userDeposit);

            // Перенаправляємо користувача на головну
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> All()
		{
            // Беремо поточного юзера
			var currentUser = await _userManager.GetUserAsync(User);

            // Дістаємо його депозити
            IEnumerable<UserDeposit> userDeposits = await _userDepositRepository.GetByUserIdAsync(currentUser.Id);

			return View(userDeposits);
        }
    }
}
