using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Iterfaces
{
    public interface IDepositRepository
    {
        Task<IEnumerable<Deposit>> GetAll();
        Task<Deposit> GetByIdAsync(int Id);

        bool Add(Deposit deposit);
        bool Update(Deposit deposit);
        bool Delete(Deposit deposit);
        bool Save();
    }
}
