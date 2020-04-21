using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
	}
}