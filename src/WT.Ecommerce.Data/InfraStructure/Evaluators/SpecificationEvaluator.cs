using System.Linq;
using WT.Ecommerce.Domain.Models;
using WT.Ecommerce.Data.InfraStructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace WT.Ecommerce.Data.InfraStructure.Evaluators
{
    public class SpecificationEvaluator<T, K> where T : BaseEntity<K>
    {
		public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T, K> specification)
		{
			var query = inputQuery;

			// modify the IQueryable using the specification's criteria expression
			query = specification.Criterias.Aggregate(query,
				(current, criteria) => current.Where(criteria));

			// Includes all expression-based includes
			query = specification.Includes.Aggregate(query,
				(current, include) => current.Include(include));

			// Include any string-based include statements
			query = specification.IncludeStrings.Aggregate(query,
				(current, include) => current.Include(include));

			// Apply ordering if expressions are set
			if (specification.OrderBy != null)
			{
				query = query.OrderBy(specification.OrderBy);
			}
			else if (specification.OrderByDescending != null)
			{
				query = query.OrderByDescending(specification.OrderByDescending);
			}

			// Apply paging if enabled
			if (specification.IsPagingEnabled)
			{
				query = query.Skip(specification.Skip)
					.Take(specification.Take);
			}
			return query;
		}
	}
}
