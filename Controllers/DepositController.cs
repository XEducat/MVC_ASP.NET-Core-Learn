using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Iterfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
	public class DepositController : Controller
	{
		private readonly IDepositRepository _depositRepository;

		public DepositController(AppDbContext context, IDepositRepository depositRepository)
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
	}
}
