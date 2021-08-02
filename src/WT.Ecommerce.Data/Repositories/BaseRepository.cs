using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT.Ecommerce.Data.InfraStructure.Evaluators;
using WT.Ecommerce.Data.Repositories.Interfaces;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data.Repositories
{
    public class BaseRepository<T, K> : IRepository<T, K> where T : BaseEntity<K>
	{
		protected readonly ApplicationDbContext _dbContext;

		public BaseRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual async Task<T> GetByIdAsync(K id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<IReadOnlyList<T>> ListAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T, K> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public async Task<int> CountAsync(ISpecification<T, K> spec)
		{
			return await ApplySpecification(spec).CountAsync();
		}

		public async Task<T> AddAsync(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task<T> UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public virtual async Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		private IQueryable<T> ApplySpecification(ISpecification<T, K> spec)
		{
			return SpecificationEvaluator<T, K>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
		}


	}
}
