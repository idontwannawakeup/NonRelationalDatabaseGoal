using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class TeamService : GenericService<Team>
{
    public TeamService(CosmosClient client) : base(client.GetTeamsContainer())
    {
        UsersContainer = client.GetUsersContainer();
        ProjectsContainer = client.GetProjectsContainer();
        TicketsContainer = client.GetTicketsContainer();
    }

    protected Container UsersContainer { get; }
    protected Container ProjectsContainer { get; }
    protected Container TicketsContainer { get; }

    public override async Task CreateAsync(Team team)
    {
        Models.User leader = await UsersContainer.ReadItemAsync<Models.User>(
            team.LeaderId,
            new PartitionKey(team.LeaderId));

        leader.Teams.Add(team.Id);
        team.Members.Add(leader.Id);
        await base.CreateAsync(team);
        await UsersContainer.UpsertItemAsync(leader, new PartitionKey(leader.Id));
    }

    public override async Task DeleteAsync(string id)
    {
        Team team = await base.GetByIdAsync(id);
        var projects = await ProjectsContainer.GetItemLinqQueryable<Project>()
            .Where(project => project.TeamId == team.Id)
            .ToFeedIterator()
            .ReadAllAsync();

        foreach (var project in projects)
        {
            var tickets = await TicketsContainer.GetItemLinqQueryable<Ticket>()
                .Where(ticket => ticket.ProjectId == project.Id)
                .ToFeedIterator()
                .ReadAllAsync();

            foreach (var ticket in tickets)
            {
                if (ticket.ExecutorId is not null)
                {
                    Models.User user = await UsersContainer.ReadItemAsync<Models.User>(
                        ticket.ExecutorId,
                        new PartitionKey(ticket.ExecutorId));

                    user.AssignedTickets.Remove(ticket.Id);
                    user.Teams.Remove(team.Id);
                    await UsersContainer.UpsertItemAsync(user, new PartitionKey(user.Id));
                }

                await TicketsContainer.DeleteItemAsync<Ticket>(
                    ticket.Id,
                    new PartitionKey(ticket.Id));
            }

            await ProjectsContainer.DeleteItemAsync<Project>(
                project.Id,
                new PartitionKey(project.Id));
        }

        var members = await UsersContainer.GetItemLinqQueryable<Models.User>()
            .Where(user => user.Teams.Contains(team.Id))
            .ToFeedIterator()
            .ReadAllAsync();

        foreach (var member in members)
        {
            member.Teams.Remove(team.Id);
            await UsersContainer.UpsertItemAsync(member, new PartitionKey(member.Id));
        }

        await base.DeleteAsync(id);
    }

    public async Task<IEnumerable<Team>> GetAsync(TeamParameters parameters) =>
        await Container.GetItemLinqQueryable<Team>()
            .ApplyFiltering(parameters)
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();

    public async Task<IEnumerable<Models.User>> GetMembersAsync(string teamId)
    {
        Team team = await base.GetByIdAsync(teamId);
        return await UsersContainer.GetItemLinqQueryable<Models.User>()
            .Where(user => team.Members.Contains(user.Id))
            .ToFeedIterator()
            .ReadAllAsync();
    }

    public async Task AddMemberAsync(string teamId, string memberId)
    {
        Team team = await base.GetByIdAsync(teamId);
        Models.User member = await UsersContainer.ReadItemAsync<Models.User>(
            memberId,
            new PartitionKey(memberId));

        team.Members.Add(memberId);
        member.Teams.Add(teamId);
        await base.UpdateAsync(team);
        await UsersContainer.UpsertItemAsync(member, new PartitionKey(member.Id));
    }

    public async Task DeleteMemberAsync(string teamId, string memberId)
    {
        Team team = await base.GetByIdAsync(teamId);
        Models.User member = await UsersContainer.ReadItemAsync<Models.User>(
            memberId,
            new PartitionKey(memberId));

        if (team.LeaderId == member.Id)
        {
            team.LeaderId = null;
        }

        team.Members.Remove(memberId);
        member.Teams.Remove(teamId);
        await base.UpdateAsync(team);
        await UsersContainer.UpsertItemAsync(member, new PartitionKey(member.Id));
    }
}
