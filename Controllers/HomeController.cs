﻿using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Diagnostics;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Route("/NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
