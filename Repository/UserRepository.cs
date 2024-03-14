using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext appDbContext)
        {
			_context = appDbContext;
		}

		public async Task<IEnumerable<AppUser>> GetAllUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<AppUser> GetUserById(string Id)
		{
			return await _context.Users.FindAsync(Id);
		}

		public bool Add(AppUser user)
		{
			_context.Add(user);
			return Save();
		}

		public bool Delete(AppUser user)
		{
			_context.Remove(user);
			return Save();
		}

		public bool Update(AppUser user)
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
