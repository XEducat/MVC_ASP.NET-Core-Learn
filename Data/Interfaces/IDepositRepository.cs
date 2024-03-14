using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
	public interface IDepositRepository : IBaseRepository<Deposit>
	{
		Task<Deposit> GetByIdAsyncNoTraking(int Id);
	}
}
