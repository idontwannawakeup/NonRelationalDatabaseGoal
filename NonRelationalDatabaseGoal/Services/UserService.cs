using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class UserService : GenericService<Models.User>
{
    public UserService(CosmosClient client) : base(client.GetUsersContainer())
    {
    }

    public async Task<IEnumerable<Models.User>> GetAsync(UserParameters parameters) =>
        await Container.GetItemLinqQueryable<Models.User>()
            .ApplyFiltering(parameters)
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();
}
