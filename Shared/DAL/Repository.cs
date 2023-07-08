using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.DAL.Interfaces;

namespace Shared.DAL
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected readonly DbContext context;
		protected readonly DbSet<TEntity> entities;

		public Repository(DbContext context)
		{
			this.context = context;
			entities = this.context.Set<TEntity>();
		}

		public TEntity Get(int id)
		{
			return entities.Find(id);
		}

		public TEntity Get(string id)
		{
			return entities.Find(id);
		}
		public TEntity Get(Guid id)
		{
			return entities.Find(id);
		}

		public IEnumerable<TEntity> GetAll()
		{
			return entities.ToList();
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
		{
			return entities.Where(predicate);
		}

		public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
		{
			return entities.SingleOrDefault(predicate);
		}

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => await entities.FirstOrDefaultAsync(predicate);


        public void Add(TEntity entity)
		{
			entities.Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			this.entities.AddRange(entities);
		}

		public void Remove(TEntity entity)
		{
			entities.Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			this.entities.RemoveRange(entities);
		}

        Task<TEntity> IRepository<TEntity>.GetAsync(int id)
        {
			return Task.FromResult(entities.SingleOrDefault());
        }

        Task<TEntity> IRepository<TEntity>.GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IRepository<TEntity>.GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IRepository<TEntity>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IRepository<TEntity>.FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task IRepository<TEntity>.AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<TEntity>.AddRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}