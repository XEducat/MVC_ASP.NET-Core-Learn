using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
		}


        public IActionResult Login()
		{
			var responce = new LoginViewModel();
			return View(responce);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if(!ModelState.IsValid) return View(loginViewModel);

			var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

			if (user != null)
			{
				// User if found, check password
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
				if (passwordCheck)
				{
					// Password correct, sign in
					var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Home");
					}
				}
				// Password is incorrect
				TempData["Error"] = "Incorrect password. Please try again";
				return View(loginViewModel);
			}
			// User not found
			TempData["Error"] = "Wrong credentails. This user not found";
			return View(loginViewModel);
		}

        public IActionResult Register()
        {
            var responce = new RegisterViewModel();
            return View(responce);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                // We have an existing account
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

			var newUser = new AppUser()
			{
                UserName = registerViewModel.UserName,
                Email  = registerViewModel.EmailAddress,
				Address = new Address() 
				{
					City = registerViewModel.City,
					State = registerViewModel.State,
					Street = registerViewModel.Street
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

			if (!newUserResponse.Succeeded)
			{
                TempData["Error"] = newUserResponse.Errors.First().Description;
                return View(registerViewModel);
            }


            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            return RedirectToAction("Index", "Deposit");
        }

		public async Task<IActionResult> LogOut()
		{
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
