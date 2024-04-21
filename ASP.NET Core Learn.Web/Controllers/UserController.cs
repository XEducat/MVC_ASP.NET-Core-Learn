using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> About()
        {
            // Получаем пользователя за именем
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return View("Error");
            }

            var detailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                //SelectedDeposits = user.Deposits
            };

            return View(detailViewModel);
        }
    }
}
