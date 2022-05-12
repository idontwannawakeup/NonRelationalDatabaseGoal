using System.Linq.Expressions;
using NonRelationalDatabaseGoal.Enums;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Extensions;

public static class OrderingExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> queryable, QueryStringParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            return queryable;
        }

        var command = parameters.Order switch
        {
            Order.Ascending => "OrderBy",
            Order.Descending => "OrderByDescending"
        };

        var type = typeof(T);
        var property = type.GetProperty(parameters.OrderBy)!;
        var parameter = Expression.Parameter(type, "p");
        var resultExpression = Expression.Call(
            typeof(Queryable),
            command,
            new Type[] { type, property.PropertyType },
            queryable.Expression,
            Expression.Quote(Expression.Lambda(
                Expression.MakeMemberAccess(parameter, property),
                parameter)));

        return queryable.Provider.CreateQuery<T>(resultExpression);
    }
}
