using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    /// <summary>
    /// Контролер для управління шаблонами депозитів.
    /// </summary>
    [Authorize]
    public class DepositTemplateController : Controller
	{
		private readonly IDepositRepository _depositRepository;

        public DepositTemplateController(IDepositRepository depositRepository)
		{
			_depositRepository = depositRepository;
        }

        [HttpGet("Open")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
		{
			IEnumerable<DepositTemplate> deposits = await _depositRepository.GetAll();

			return View(deposits);
		}

        public IActionResult Create()
        {
            var editVM = new EditDepositViewModel();

            return View(editVM);
        }

        [HttpPost]
        public IActionResult Create(EditDepositViewModel depositVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit deposit");
                return View(depositVM);
            }

            if (depositVM.Terms.Any(t => t.NumberMonths < 1))
            {
                TempData["Error"] = "Failed to add. Terms must be greatest than 0";
                return View("Create", depositVM);
            }

            var deposit = new DepositTemplate()
            {
                Title = depositVM.Title,
                ShortDescription = depositVM.ShortDescription,
                Replenishment = depositVM.Replenishment,
                InterestPayment = depositVM.InterestRate,
                Terms = depositVM.Terms,
                InterestRateEarlyClosure = depositVM.InterestRateEarlyClosure,
                InterestRateNoEarlyClosure = depositVM.InterestRateNoEarlyClosure,
            };

            _depositRepository.Add(deposit);
            return RedirectToAction("Index");
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
                Terms = deposit.Terms,
                InterestRateEarlyClosure = deposit.InterestRateEarlyClosure,
                InterestRateNoEarlyClosure = deposit.InterestRateNoEarlyClosure,
            };

            return View(depositVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDepositViewModel depositVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit deposit");
				return View("Edit", depositVM);
			}

			if(depositVM.Terms.Any(t => t.NumberMonths < 1))
			{
                TempData["Error"] = "Failed to edit. Terms must be greatest than 0";
                return View("Edit", depositVM);
            }

			var userDeposit = await _depositRepository.GetByIdAsyncNoTraking(id);

			if (userDeposit != null)
			{
				var deposit = new DepositTemplate()
				{
					Id = id,
					Title = depositVM.Title,
					ShortDescription = depositVM.ShortDescription,
					Replenishment = depositVM.Replenishment,
					InterestPayment = depositVM.InterestRate,
					Terms = depositVM.Terms,
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
			}

			return RedirectToAction("Index");
		}
    }
}
