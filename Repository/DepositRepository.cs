using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Iterfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Repository
{
	public class DepositRepository : IDepositRepository
	{
		private readonly AppDbContext _context;

		public DepositRepository(AppDbContext appDbContext)
        {
			_context = appDbContext;
		}

        public bool Add(Deposit deposit)
		{
			_context.Add(deposit);
			return Save();
		}

		public bool Delete(Deposit deposit)
		{
			_context.Remove(deposit);
			return Save();
		}

        public async Task<IEnumerable<Deposit>> GetAll()
		{
			return await _context.Deposits.Include(t => t.Terms).ToListAsync();
		}

		public async Task<Deposit> GetByIdAsync(int Id)
		{
			return await _context.Deposits.Include(t => t.Terms).FirstOrDefaultAsync(d => d.Id == Id);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Deposit deposit)
		{
			_context.Update(deposit);
			return Save();
		}
	}
}
