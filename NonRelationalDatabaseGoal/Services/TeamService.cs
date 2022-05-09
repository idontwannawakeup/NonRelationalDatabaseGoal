using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Services;

public class TeamService : GenericService<Team>
{
    public TeamService(CosmosClient client) : base(client.GetTeamsContainer())
    {
    }
}
