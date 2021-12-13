using System.Collections.Generic;
using System.Threading.Tasks;
using WT.Ecommerce.Data.InfraStructure.Evaluators;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data.Repositories.Interfaces
{
    public interface IRepository<T,K> where T : BaseEntity<K>
    {
		Task<T> GetByIdAsync(K id);
		Task<IReadOnlyList<T>> ListAllAsync();
		Task<IReadOnlyList<T>> ListAsync(ISpecification<T, K> spec);
		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task<int> CountAsync(ISpecification<T, K> spec);
	}
}
