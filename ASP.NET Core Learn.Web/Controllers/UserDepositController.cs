using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Security.Claims;

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

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByNameAsync(userName);
            if (currentUser == null)
                return RedirectToAction("Error", "Home"); // Якщо користувача не знайдено

            var deposit = await _depositRepository.GetByIdAsync(viewModel.DepositId);
            if (deposit == null)
                return RedirectToAction("Error", "Home"); // Якщо депозита не знайдено

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
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByNameAsync(userName);

            // Дістаємо його депозити
            IEnumerable<UserDeposit> userDeposits = await _userDepositRepository.GetDepositsByUserIdAsync(currentUser.Id);

			return View(userDeposits);
        }

        [HttpGet]
        public async Task<IActionResult> ReplenishmentDeposit(int id)
        {
            var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(id);

            // Передача необхідних данних депозиту в ViewBag
            ViewBag.UserDepositId = id; 
            ViewBag.SelectedDepositTitle = selectedUserDeposit.Title;
            return View();
        }

        // TODO: Можливо варто додати дату останнього поповнення та винести дані в ViewModel
        [HttpPost]
        public async Task<IActionResult> ReplenishmentDeposit(decimal replenishmentAmount, int Id)
        {
            if (replenishmentAmount <= 0) return RedirectToAction("ReplenishmentDeposit");

            var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(Id);

            // Оновлення суму депозиту користувача, додаючи введену суму поповнення
            selectedUserDeposit.Amount += replenishmentAmount;
            _userDepositRepository.Update(selectedUserDeposit);

            // Перенаправте користувача на іншу сторінку або відобразіть повідомлення про успішне поповнення
            return RedirectToAction("All");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(id);

            return View(selectedUserDeposit);
        }
    }
}
