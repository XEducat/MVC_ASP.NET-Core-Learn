using Microsoft.AspNetCore.Mvc;

namespace MVC_ASP.NET_Core_Learn.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
