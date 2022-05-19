using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Interfaces.Services;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class UserService : GenericService<AppUser>, IUserService
{
    public UserService(CosmosClient client) : base(client.GetUsersContainer())
    {
    }

    public async Task<IEnumerable<AppUser>> GetAsync(UserParameters parameters) =>
        await Container.GetItemLinqQueryable<AppUser>()
            .ApplyFiltering(parameters)
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();
}
