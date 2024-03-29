using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Repository
{
	public class DepositTemplateRepository : IDepositRepository
	{
		private readonly AppDbContext _context;

		public DepositTemplateRepository(AppDbContext appDbContext)
        {
			_context = appDbContext;
		}

        public bool Add(DepositTemplate deposit)
		{
			_context.Add(deposit);
			return Save();
		}

		public bool Delete(DepositTemplate deposit)
		{
			_context.Remove(deposit);
			return Save();
		}

        public async Task<IEnumerable<DepositTemplate>> GetAll()
		{
			return await _context.DepositTemplates.Include(t => t.Terms).ToListAsync();
		}

		public async Task<DepositTemplate> GetByIdAsync(int Id)
		{
			return await _context.DepositTemplates.Include(t => t.Terms).FirstOrDefaultAsync(d => d.Id == Id);
		}

        public async Task<DepositTemplate> GetByIdAsyncNoTraking(int Id)
        {
            return await _context.DepositTemplates.Include(t => t.Terms).AsNoTracking().FirstOrDefaultAsync(d => d.Id == Id);
        }

        public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(DepositTemplate deposit)
		{
			_context.Update(deposit);
			return Save();
		}
	}
}
