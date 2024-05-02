using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Repository
{
    public class UserRepository : UserManager<AppUser>, IUserRepository
    {
        public UserRepository(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, 
                              IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }

        public async Task<bool> AddToBalanceAsync(AppUser user, decimal amount)
        {
            if (user == null)
            {
                throw new ArgumentException($"User '{user}' is null.");
            }

            // Додати суму до депозиту користувача
            user.Balance += amount;
            var result = await UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task SubtractFromBalanceAsync(AppUser user, decimal amount)
        {
            if (user == null)
            {
                throw new ArgumentException($"User '{user}' is null.");
            }

            // Перевірити, чи користувач має достатньо коштів на депозиті для віднімання
            if (user.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            // Відняти суму від депозиту користувача
            user.Balance -= amount;
            await UpdateAsync(user);
        }
    }
}
