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

        return parameters.Order switch
        {
            Order.Ascending => queryable.OrderBy(_ => parameters.OrderBy),
            Order.Descending => queryable.OrderByDescending(_ => parameters.OrderBy)
        };
    }
}
