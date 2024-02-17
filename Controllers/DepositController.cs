using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
	public class DepositController : Controller
	{
		private readonly AppDbContext _context;

		public DepositController(AppDbContext context)
        {
			_context = context;
		}

        public IActionResult Index()
		{
			var deposits = _context.Deposits.Include(t => t.Term).ToList();

			return View(deposits);
		}


		public IActionResult Detail(int id)
		{
			Deposit deposit = _context.Deposits.FirstOrDefault(t => t.Id == id);
			//UserTask deposit = _context.UserTusks.FirstOrDefault(t => t.Id == id);

			return View(deposit);
		}
	}
}
