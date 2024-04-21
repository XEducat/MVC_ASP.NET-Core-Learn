using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.Services;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
        private readonly ApiService _apiService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApiService apiService)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _apiService = apiService;
        }

        // КОСТЫЛЬ (перенаправление атруиббута Authorize почему-то работает только на /Account/Login)
        [HttpGet]
        public IActionResult login()
		{
			return View("Index");
		}

        // TODO: Додати Login  та можливість входу за логіном
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(/*[Bind(Prefix = "l")]*/ LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Index", new AccountViewModel() { LoginViewModel = model });
        //    }

        //    var user = await _userManager.FindByEmailAsync(model.EmailAddress);

        //    if (user != null)
        //    {
        //        if (await AuthetificationAsync(user, model.Password))
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Wrong credentails. This user not found";
        //    }

        //    return View("Index", new AccountViewModel() { LoginViewModel = model });
        //}

        // TODO:
        // 1. Сделать нормальную обработку ошибок для сайта.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new AccountViewModel() { LoginViewModel = model });
            }

            try
            {
                // Вызываем метод API для аутентификации
                var response = await _apiService.Login(model.Username, model.Password);

                if (response != null)
                {
                    // Добавляем токен в куки
                    Response.Cookies.Append("access_cookie", response.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(response.Expires) // Устанавливаем срок действия токена
                    });

                    // Успешная аутентификация, перенаправляем пользователя на главную страницу
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Если аутентификация не удалась, устанавливаем соответствующее сообщение об ошибке
                    ViewBag.Error = "Wrong credentials. This user not found";
                    return View("Index", new AccountViewModel() { LoginViewModel = model });
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index", new AccountViewModel() { LoginViewModel = model });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                return RedirectToAction("Error", "Home"); // Якщо депозита не знайдено
            }
        }


        private async Task<bool> AuthetificationAsync(AppUser user, string password)
        {
            // User if found, check password
            var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
            if (passwordCheck)
            {
                // Password correct, sign in
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return result.Succeeded;
            }
            else
            {
                // Password is incorrect
                ViewBag.Error = "Incorrect password. Please try again";
            }

            return false;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(/*[Bind(Prefix = "r")]*/ RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new AccountViewModel() { RegisterViewModel = model });
            }

            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user != null)
            {
                // We have an existing account
                ViewBag.Error = "This email address is already in use";
                return View("Index", new AccountViewModel() { RegisterViewModel = model });
            }

            var newUser = new AppUser()
            {
                UserName = model.UserName,
                Email = model.EmailAddress,
                Address = new Address()
                {
                    City = model.City,
                    State = model.State,
                    Street = model.Street
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                if (await AuthetificationAsync(newUser, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    ViewBag.Error = "User not auto Authentificated";
                }
            }
            else
            {
                ViewBag.Error = newUserResponse.Errors.First().Description;
            }

            return View("Index", new AccountViewModel() { RegisterViewModel = model });
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
		{
            // #CANBETRASH
            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("access_cookie");
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Delete()
		{
			// Беремо поточного юзера
			var currentUser = await _userManager.GetUserAsync(User);

			// Після цього видалення користувача може бути проведено як зазвичай
			var deletedUserResponse = await _userManager.DeleteAsync(currentUser);

			if (!deletedUserResponse.Succeeded)
			{
				TempData["Error"] = deletedUserResponse.Errors.First().Description;
				return View("Error");
			}

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
	}
}