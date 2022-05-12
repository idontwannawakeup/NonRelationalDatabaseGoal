using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Extensions.Filtering;

public static class ProjectFilteringExtensions
{
    public static IQueryable<Project> ApplyFiltering(
        this IQueryable<Project> projects,
        ProjectParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.TeamId))
        {
            projects = projects.Where(project => project.TeamId == parameters.TeamId);
        }

        if (!string.IsNullOrWhiteSpace(parameters.Title))
        {
            projects = projects.Where(project => project.Title.Contains(parameters.Title));
        }

        return projects;
    }
}
