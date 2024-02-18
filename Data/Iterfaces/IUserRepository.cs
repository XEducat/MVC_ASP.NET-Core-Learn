using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Iterfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAll();
        //Task<User> GetByIdAsync(int Id);

        bool Add(AppUser deposit);
        bool Update(AppUser deposit);
        bool Delete(AppUser deposit);
        bool Save();
    }
}
