using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

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
            var user = await _userManager.GetUserAsync(User);

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
