using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
	public interface IUserDepositRepository : IBaseRepository<UserDeposit>
	{
		Task<IEnumerable<UserDeposit>> GetDepositsByUserIdAsync(string UserId);
	}
}
