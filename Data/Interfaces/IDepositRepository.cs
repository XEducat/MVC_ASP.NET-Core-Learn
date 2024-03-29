using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
	public interface IDepositRepository : IBaseRepository<DepositTemplate>
	{
		Task<DepositTemplate> GetByIdAsyncNoTraking(int Id);
	}
}
