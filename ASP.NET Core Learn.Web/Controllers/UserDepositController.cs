using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
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
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserDepositController(IDepositRepository depositRepository, IUserDepositRepository userDepositRepository, IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _depositRepository = depositRepository;
            _userManager = userManager;
			_userDepositRepository = userDepositRepository;
            _userRepository = userRepository;
        }


        // TODO: Зробити зберігання заповнених данних
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
                return View(viewModel);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Error", "Home"); // Якщо користувача не знайдено

            var deposit = await _depositRepository.GetByIdAsync(viewModel.DepositId);
            if (deposit == null)
                return RedirectToAction("Error", "Home"); // Якщо депозита не знайдено

            if(currentUser.Balance < viewModel.Amount)
            {
                string returnUrl = Url.Action("OrderForm", "UserDeposit", new { viewModel });
                return RedirectToAction("ReplenishBalance", "User", new { recommendedAmount = viewModel.Amount - currentUser.Balance, returnUrl}); // Якщо не вистачає грошей на балансі
            }
            ////TODO: Повернення сторінки з модальним вікном про нестачу балансу
            //if (currentUser.Balance < viewModel.Amount)
            //{
            //    TempData["InsufficientFunds"] = true;
            //    return View(viewModel);
            //}

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

            // Знымаємо гроші з балансу та додаємо userDeposit в БД
            await _userRepository.SubtractFromBalanceAsync(currentUser, viewModel.Amount);
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
            IEnumerable<UserDeposit> userDeposits = await _userDepositRepository.GetDepositsByUserIdAsync(currentUser.Id);

			return View(userDeposits);
        }

		// TODO: Зробити поповнення доступним лише при оформленні депозиту
		[HttpGet]
        public async Task<IActionResult> ReplenishmentDeposit(int id)
        {
            var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(id);

            //TODO Перобити під модель
            // Передача необхідних данних депозиту в ViewBag
            ViewBag.UserDepositId = id; 
            ViewBag.SelectedDepositTitle = selectedUserDeposit.Title;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReplenishmentDeposit(decimal replenishmentAmount, int Id)
        {
            if (replenishmentAmount <= 0) return RedirectToAction("ReplenishmentDeposit");

			// TODO: Переробити щоб прибрати дубляж коду
			// Беремо поточного юзера
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser == null)
				return RedirectToAction("Error", "Home"); // Якщо користувача не знайдено

			var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(Id);

            if(currentUser.Balance < replenishmentAmount)
            {
				ModelState.AddModelError(string.Empty, "На вашому рахунку недостатньо коштів для поповнення.");
                ViewBag.UserDepositId = Id;
                ViewBag.SelectedDepositTitle = selectedUserDeposit.Title;
                return View();
			}

			// Оновлення суму депозиту користувача, додаючи введену суму поповнення
			await _userRepository.SubtractFromBalanceAsync(currentUser, replenishmentAmount);
			selectedUserDeposit.Amount += replenishmentAmount;
            _userDepositRepository.Update(selectedUserDeposit);

			// Перенаправте користувача на іншу сторінку або відобразіть повідомлення про успішне поповнення
			return RedirectToAction("All");
        }


		public async Task<IActionResult> WithdrawDeposit(int Id)
		{
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser == null)
				return RedirectToAction("Error", "Home"); // Якщо користувача не знайдено\
			var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(Id);

			// Оновлення балансу користувача, видалення депозиту
			await _userRepository.AddToBalanceAsync(currentUser, selectedUserDeposit.Amount);
			_userDepositRepository.Delete(selectedUserDeposit);

			// Встановлення значення ViewBag для показу модального вікна
			TempData["DepositWithdrawn"] = true;
			return RedirectToAction("All");
		}

		public async Task<IActionResult> Detail(int id)
        {
            var selectedUserDeposit = await _userDepositRepository.GetByIdAsync(id);

            return View(selectedUserDeposit);
        }
    }
}
