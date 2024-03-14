using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
	public interface IUserRepository : IBaseRepository<AppUser>
	{
		Task<AppUser> GetUserById(string Id);
	}
}
