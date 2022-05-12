using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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
        Models.User leader = await UsersContainer.ReadItemAsync<Models.User>(
            team.LeaderId,
            new PartitionKey(team.LeaderId));

        leader.Teams.Add(team.Id);
        team.Members.Add(leader.Id);
        await base.CreateAsync(team);
        await UsersContainer.UpsertItemAsync(leader, new PartitionKey(leader.Id));
    }

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

        team.Members.Remove(memberId);
        member.Teams.Remove(teamId);
        await base.UpdateAsync(team);
        await UsersContainer.UpsertItemAsync(member, new PartitionKey(member.Id));
    }
}
