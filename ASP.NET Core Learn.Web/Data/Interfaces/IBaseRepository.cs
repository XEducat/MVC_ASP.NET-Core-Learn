namespace MVC_ASP.NET_Core_Learn.Data.Interfaces
{
	public interface IBaseRepository<TEntity>
	{
		Task<IEnumerable<TEntity>> GetAll();
		Task<TEntity> GetByIdAsync(int Id);

		bool Add(TEntity entity);
		bool Update(TEntity entity);
		bool Delete(TEntity entity);
		bool Save();
	}

}
