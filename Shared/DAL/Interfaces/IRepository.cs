using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.DAL.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity Get(int id);
		TEntity Get(string id);
		TEntity Get(Guid id);
		IEnumerable<TEntity> GetAll();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
		TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		#region Async methods
		Task<TEntity> GetAsync(int id);
		Task<TEntity> GetAsync(string id);
		Task<TEntity> GetAsync(Guid id);
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
		Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

		Task AddAsync(TEntity entity);
		Task AddRangeAsync(IEnumerable<TEntity> entities);
		#endregion

		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
	}
}
