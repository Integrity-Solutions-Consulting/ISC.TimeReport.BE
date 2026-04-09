namespace isc_tmr_backend.Infrastructure.Extensions;

using System.Linq.Expressions;
using System.Reflection;

public static class QueryableExtensions
{
    public static (IQueryable<T> Query, int TotalCount) ApplyPagination<T>(
        this IQueryable<T> query,
        int page,
        int take)
    {
        int totalCount = query.Count();
        
        int skip = (page - 1) * take;
        
        var pagedQuery = query.Skip(skip).Take(take);
        
        return (pagedQuery, totalCount);
    }

    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        string? sortBy,
        bool ascending = true)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        PropertyInfo? property = typeof(T).GetProperty(
            sortBy, 
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property is null)
            return query;

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
        LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);

        string methodName = ascending ? "OrderBy" : "OrderByDescending";

        Expression methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), property.PropertyType],
            query.Expression,
            Expression.Quote(orderByExpression));

        return query.Provider.CreateQuery<T>(methodCallExpression);
    }

    public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> query,
        string? searchTerm,
        params Expression<Func<T, string>>[] propertyExpressions)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || propertyExpressions.Length == 0)
            return query;

        searchTerm = searchTerm.Trim().ToLower();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        
        Expression? combinedExpression = null;

        foreach (var propertyExpression in propertyExpressions)
        {
            Expression propertyBody = Expression.Invoke(propertyExpression, parameter);
            
            MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
            MethodCallExpression toLowerCall = Expression.Call(propertyBody, toLowerMethod);

            ConstantExpression searchValue = Expression.Constant(searchTerm, typeof(string));
            
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
            MethodCallExpression containsCall = Expression.Call(toLowerCall, containsMethod, searchValue);

            combinedExpression = combinedExpression is null
                ? (Expression)containsCall
                : Expression.OrElse(combinedExpression, containsCall);
        }

        if (combinedExpression is null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        return query.Where(lambda);
    }
}
