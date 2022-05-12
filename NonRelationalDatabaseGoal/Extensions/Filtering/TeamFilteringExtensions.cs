using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Extensions.Filtering;

public static class TeamFilteringExtensions
{
    public static IQueryable<Team> ApplyFiltering(
        this IQueryable<Team> teams,
        TeamParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.MemberId))
        {
            teams = teams.Where(team => team.Members.Contains(parameters.MemberId));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            teams = teams.Where(team => team.Name.Contains(parameters.Name));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Specialization))
        {
            teams = teams.Where(
                team => team.Specialization != null
                        && team.Specialization.Contains(parameters.Specialization));
        }

        return teams;
    }
}
