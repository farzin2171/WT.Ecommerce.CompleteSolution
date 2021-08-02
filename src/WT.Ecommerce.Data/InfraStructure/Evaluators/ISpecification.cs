using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data.InfraStructure.Evaluators
{
    public interface ISpecification<T, K> where T : BaseEntity<K>
	{
		List<Expression<Func<T, bool>>> Criterias { get; }
		List<Expression<Func<T, object>>> Includes { get; }
		List<string> IncludeStrings { get; }
		Expression<Func<T, object>> OrderBy { get; }
		Expression<Func<T, object>> OrderByDescending { get; }

		int Take { get; }
		int Skip { get; }
		bool IsPagingEnabled { get; }
	}
}
