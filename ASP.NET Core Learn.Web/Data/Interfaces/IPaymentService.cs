using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Поповнює баланс вказаного користувача на вказану суму.
        /// </summary>
        /// <param name="user">Користувач, баланс якого потрібно поповнити.</param>
        /// <param name="amount">Сума для поповнення балансу.</param>
        /// <returns>Повертає true, якщо баланс успішно поповнено; у протилежному випадку повертає false.</returns>
        /// <exception cref="ArgumentNullException">Виникає, якщо переданий користувач дорівнює null.</exception>
        /// <exception cref="ArgumentException">Виникає, якщо передана сума менша або дорівнює нулю.</exception>
        Task<bool> ReplenishBalanceAsync(AppUser user, decimal amount);
    }
}
