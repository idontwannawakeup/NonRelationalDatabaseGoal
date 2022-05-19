using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Interfaces.Services;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class ProjectService : GenericService<Project>, IProjectService
{
    public ProjectService(CosmosClient client) : base(client.GetProjectsContainer())
    {
        UsersContainer = client.GetUsersContainer();
        TeamsContainer = client.GetTeamsContainer();
        TicketsContainer = client.GetTicketsContainer();
    }

    protected Container UsersContainer { get; }
    protected Container TeamsContainer { get; }
    protected Container TicketsContainer { get; }

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

        var tickets = await TicketsContainer.GetItemLinqQueryable<Ticket>()
            .Where(ticket => ticket.ProjectId == project.Id)
            .ToFeedIterator()
            .ReadAllAsync();

        foreach (var ticket in tickets)
        {
            if (ticket.ExecutorId is not null)
            {
                Models.AppUser user = await UsersContainer.ReadItemAsync<AppUser>(
                    ticket.ExecutorId,
                    new PartitionKey(ticket.ExecutorId));

                user.AssignedTickets.Remove(ticket.Id);
                await UsersContainer.UpsertItemAsync(user, new PartitionKey(user.Id));
            }

            await TicketsContainer.DeleteItemAsync<Ticket>(
                ticket.Id,
                new PartitionKey(ticket.Id));
        }

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
