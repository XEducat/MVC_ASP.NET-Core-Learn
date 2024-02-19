using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Iterfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
	public class DepositController : Controller
	{
		private readonly IDepositRepository _depositRepository;

		public DepositController(IDepositRepository depositRepository)
        {
			_depositRepository = depositRepository;
		}

        public async Task<IActionResult> Index()
		{
			IEnumerable<Deposit> deposits = await _depositRepository.GetAll();

			return View(deposits);
		}


		public async Task<IActionResult> Detail(int id)
		{
			Deposit deposit = await _depositRepository.GetByIdAsync(id);

			return View(deposit);
		}

		public IActionResult Create()
		{
			return View();
		}

		public async Task<IActionResult> Edit(int id)
		{
			var deposit = await _depositRepository.GetByIdAsync(id);
			if(deposit == null) return View("Error");
			var depositVM = new EditDepositViewModel()
			{
				Id = id,
				Title = deposit.Title,
				ShortDescription = deposit.ShortDescription,
				Replenishment = deposit.Replenishment,
				InterestRate = deposit.InterestRate,
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

			var userDeposit = await _depositRepository.GetByIdAsyncNoTraking(id);

			if (userDeposit != null)
			{
				var deposit = new Deposit()
				{
					Id = id,
					Title = depositVM.Title,
					ShortDescription = depositVM.ShortDescription,
					Replenishment = depositVM.Replenishment,
					InterestRate = depositVM.InterestRate,
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
    }
}
