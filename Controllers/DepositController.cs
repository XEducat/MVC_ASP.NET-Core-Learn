using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Iterfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
	public class DepositController : Controller
	{
		private readonly IDepositRepository _depositRepository;
        private readonly AppDbContext _context; // TODO: Додати IUserDepositRepository
        private readonly UserManager<AppUser> _userManager;

        public DepositController(IDepositRepository depositRepository, UserManager<AppUser> userManager, AppDbContext context)
		{
			_depositRepository = depositRepository;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("Open")]
        public async Task<IActionResult> Index()
		{
			IEnumerable<Deposit> deposits = await _depositRepository.GetAll();

			return View(deposits);
		}

        [HttpGet("Open/{id}")]
		[Authorize]
        public async Task<IActionResult> Detail(int id)
        {
            var deposit = await _depositRepository.GetByIdAsync(id);
            if (deposit == null)
            {
                return View("Error");
            }

            UserDepositViewModel viewModel = new UserDepositViewModel
            {
                DepositId = deposit.Id,
                Title = deposit.Title,
                Terms = deposit.Terms,
                InterestRateEarlyClosure = deposit.InterestRateEarlyClosure,
                InterestRateNoEarlyClosure = deposit.InterestRateNoEarlyClosure,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeUserToDeposit(UserDepositViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Detail", viewModel);

            var currentUser = await _userManager.GetUserAsync(User);
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
                Amount = (decimal)viewModel.Amount,
                InterestRate = viewModel.InterestRate,
                IsEarlyClosureAllowed = viewModel.IsEarlyClosureAllowed,
                SelectedTerm = viewModel.SelectedTerm,
            };

            // Додаємо userDeposit до контексту даних і зберігаємо зміни
            _context.UserDeposits.Add(userDeposit);
            await _context.SaveChangesAsync();

            // Перенаправляємо користувача на головну
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
		{
			var deposit = await _depositRepository.GetByIdAsync(id);
			if (deposit == null) return View("Error");
			var depositVM = new EditDepositViewModel()
			{
				Id = id,
				Title = deposit.Title,
				ShortDescription = deposit.ShortDescription,
				Replenishment = deposit.Replenishment,
				InterestRate = deposit.InterestPayment,
				Term = deposit.Terms,
				InterestRateEarlyClosure = deposit.InterestRateEarlyClosure,
				InterestRateNoEarlyClosure = deposit.InterestRateNoEarlyClosure,
			};

			return View(depositVM);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditDepositViewModel depositVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit deposit");
				return View("Edit", depositVM);
			}

			if(depositVM.Term.Any(t => t.NumberMonths < 1))
			{
                TempData["Error"] = "Failed to edit. Terms must be greatest than 0";
                return View("Edit", depositVM);
            }

			var userDeposit = await _depositRepository.GetByIdAsyncNoTraking(id);

			if (userDeposit != null)
			{
				var deposit = new Deposit()
				{
					Id = id,
					Title = depositVM.Title,
					ShortDescription = depositVM.ShortDescription,
					Replenishment = depositVM.Replenishment,
					InterestPayment = depositVM.InterestRate,
					Terms = depositVM.Term,
					InterestRateEarlyClosure = depositVM.InterestRateEarlyClosure,
					InterestRateNoEarlyClosure = depositVM.InterestRateNoEarlyClosure,
				};

				_depositRepository.Update(deposit);
				return RedirectToAction("Index");
			}
			else
			{
				return View(depositVM);
			}
		}

		public async Task<IActionResult> Delete(int id)
		{
			var userDeposit = await _depositRepository.GetByIdAsync(id);

			if (userDeposit != null)
			{
				_depositRepository.Delete(userDeposit);
				return RedirectToAction("Index");
			}

			return View("Index");
		}
	}
}
