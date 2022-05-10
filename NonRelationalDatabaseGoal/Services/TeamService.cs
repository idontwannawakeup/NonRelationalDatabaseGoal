using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Services;

public class TeamService : GenericService<Team>
{
    protected Container UsersContainer { get; }

    public TeamService(CosmosClient client) : base(client.GetTeamsContainer())
    {
        UsersContainer = client.GetUsersContainer();
    }

    public override async Task CreateAsync(Team team)
    {
        var leaderResponse = await UsersContainer.ReadItemAsync<Models.User>(team.LeaderId, new PartitionKey(team.LeaderId));
        var leader = leaderResponse.Resource;
        leader.Teams.Add(team.Id);
        team.Members.Add(leader.Id);
        await base.CreateAsync(team);
        await UsersContainer.UpsertItemAsync(leader, new PartitionKey(leader.Id));
    }
}
