using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> queryable, QueryStringParameters parameters) =>
        queryable.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
}
