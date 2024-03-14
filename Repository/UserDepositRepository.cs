using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Repository
{
	public class UserDepositRepository : IUserDepositRepository
	{
		private readonly AppDbContext _context;

		public UserDepositRepository(AppDbContext appDbContext)
		{
			_context = appDbContext;
		}

		public async Task<IEnumerable<UserDeposit>> GetAll()
		{
			return await _context.UserDeposits.ToListAsync();
		}

		public async Task<UserDeposit> GetByIdAsync(int Id)
		{
			return await _context.UserDeposits.FindAsync(Id);
		}

		public async Task<IEnumerable<UserDeposit>> GetByUserIdAsync(string UserId)
		{
			return await _context.UserDeposits.Where(ud => ud.UserId == UserId).ToListAsync();
		}


		public bool Add(UserDeposit user)
		{
			_context.Add(user);
			return Save();
		}

		public bool Delete(UserDeposit user)
		{
			_context.Remove(user);
			return Save();
		}

		public bool Update(UserDeposit user)
		{
			_context.Update(user);
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}
	}
}
