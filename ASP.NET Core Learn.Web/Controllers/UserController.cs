using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPaymentService _paymentService;

        public UserController(UserManager<AppUser> userManager, IPaymentService paymentService)
        {
            _userManager = userManager;
            _paymentService = paymentService;
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
                Balance = user.Balance,
                //SelectedDeposits = user.Deposits
            };

            return View(detailViewModel);
        }

        [HttpGet("replenishment")]
        public async Task<IActionResult> ReplenishBalance(decimal recommendedAmount = 0, string returnUrl = null)
        {
            //TODO: Видалити або доробити переадресацію назад на оформлення
            //ViewData["ReturnUrl"] = returnUrl;
            return View(recommendedAmount);
        }

        [HttpPost("replenishment")]
        public async Task<IActionResult> AddToBalance([FromForm]decimal amount, string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var success = await _paymentService.ReplenishBalanceAsync(user, amount);

                if (success)
                {
                    // Перевірка, чи вказаний returnUrl
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        // Перенаправлення на returnUrl
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Перенаправлення на сторінку "About" користувача, якщо returnUrl не вказаний або не локальний
                        return RedirectToAction("About", "User");
                    }
                }
                else
                {
                    return BadRequest("Amount should be greater than 0.");
                }
            }
            else
            {
                return BadRequest("User not found.");
            }
        }

    }
}
