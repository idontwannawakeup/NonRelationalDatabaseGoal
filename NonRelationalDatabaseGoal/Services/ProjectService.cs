using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class ProjectService : GenericService<Project>
{
    public ProjectService(CosmosClient client) : base(client.GetProjectsContainer())
    {
        TeamsContainer = client.GetTeamsContainer();
    }

    protected Container TeamsContainer { get; }

    public override async Task CreateAsync(Project project)
    {
        Team team = await TeamsContainer.ReadItemAsync<Team>(
            project.TeamId,
            new PartitionKey(project.TeamId));

        team.Projects.Add(project.Id);
        await base.CreateAsync(project);
        await TeamsContainer.UpsertItemAsync(team, new PartitionKey(team.Id));
    }

    public override async Task DeleteAsync(string id)
    {
        Project project = await base.GetByIdAsync(id);
        Team team = await TeamsContainer.ReadItemAsync<Team>(
            project.TeamId,
            new PartitionKey(project.TeamId));

        team.Projects.Remove(project.Id);
        await base.DeleteAsync(project.Id);
        await TeamsContainer.UpsertItemAsync(team, new PartitionKey(team.Id));
    }

    public async Task<IEnumerable<Project>> GetAsync(ProjectParameters parameters) =>
        await Container.GetItemLinqQueryable<Project>()
            .ApplyFiltering(parameters)
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();
}
