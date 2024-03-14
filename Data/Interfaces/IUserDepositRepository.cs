using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
	public interface IUserDepositRepository
	{
		Task<IEnumerable<UserDeposit>> GetAll();
		Task<UserDeposit> GetByIdAsync(int Id);
		Task<IEnumerable<UserDeposit>> GetByUserIdAsync(string UserId);


		bool Add(UserDeposit deposit);
		bool Update(UserDeposit deposit);
		bool Delete(UserDeposit deposit);
		bool Save();
	}
}
