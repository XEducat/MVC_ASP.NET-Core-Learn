using Microsoft.AspNetCore.Identity;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddToBalanceAsync(AppUser user, decimal amount);
        Task SubtractFromBalanceAsync(AppUser user, decimal amount);
    }
}
