using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Iterfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        //Task<User> GetByIdAsync(int Id);

        bool Add(User deposit);
        bool Update(User deposit);
        bool Delete(User deposit);
        bool Save();
    }
}
