using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class UserService : GenericService<Models.User>
{
    public UserService(CosmosClient client) : base(client.GetUsersContainer())
    {
    }

    public async Task<IEnumerable<Models.User>> GetAsync(QueryStringParameters parameters) =>
        await Container.GetItemLinqQueryable<Models.User>()
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();
}
