using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Extensions.Filtering;

public static class UserFilteringExtensions
{
    public static IQueryable<Models.User> ApplyFiltering(
        this IQueryable<Models.User> users,
        UserParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.TeamId))
        {
            users = users.Where(user => user.Teams.Contains(parameters.TeamId));
        }

        if (!string.IsNullOrWhiteSpace(parameters.UserName))
        {
            users = users.Where(user => user.UserName.Contains(parameters.UserName));
        }

        if (!string.IsNullOrWhiteSpace(parameters.FirstName))
        {
            users = users.Where(user => user.FirstName.Contains(parameters.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(parameters.LastName))
        {
            users = users.Where(user => user.LastName.Contains(parameters.LastName));
        }

        if (!string.IsNullOrWhiteSpace(parameters.FullName))
        {
            users = users.Where(user => $"{user.FirstName} {user.LastName}".Contains(parameters.FullName));
        }

        return users;
    }
}
