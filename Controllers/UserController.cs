using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Iterfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _userRepository.Add(user);
            return RedirectToAction("Index");
        }
    }
}
