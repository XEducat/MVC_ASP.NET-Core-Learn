using Microsoft.AspNetCore.Identity;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.Repository;

namespace MVC_ASP.NET_Core_Learn.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUserRepository _userRepository;

        public PaymentService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ReplenishBalanceAsync(AppUser user, decimal amount)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (amount <= 0)
                throw new ArgumentException("Amount should be greater than 0.", nameof(amount));

            // Виконання банкіських оперій

            // Зміна балансу в БД
            return await _userRepository.AddToBalanceAsync(user, amount);
        }
    }

}
