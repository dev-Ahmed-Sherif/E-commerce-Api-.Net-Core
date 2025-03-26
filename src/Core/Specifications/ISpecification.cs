using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Functions For more cleaner code more orginized in multi Includes
        Expression<Func<T,bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        
        // Functions For Sort Data
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        // Pagination
        int Take { get; }
        int Skip { get; }
        bool IsPaginatedEnabled { get; }
        Expression<Func<T,object>> OrderBy { get; }
        Expression<Func<T,object>> OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
