using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;

namespace NonRelationalDatabaseGoal.Services;

public class UserService : GenericService<Models.User>
{
    public UserService(CosmosClient client) : base(client.GetUsersContainer())
    {
    }
}
