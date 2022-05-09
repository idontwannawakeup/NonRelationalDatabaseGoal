using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;

namespace NonRelationalDatabaseGoal.Services;

public class UserService
{
    private readonly Container _usersContainer;

    public UserService(CosmosClient client) => _usersContainer = client.GetUsersContainer();

    public IAsyncEnumerable<Models.User> GetAll() => _usersContainer.GetAll<Models.User>();

    public async Task<Models.User> GetByIdAsync(string id) => await _usersContainer.ReadItemAsync<Models.User>(
        id, new PartitionKey(id));

    public async Task CreateAsync(Models.User user) => await _usersContainer.CreateItemAsync<Models.User>(
        user, new PartitionKey(user.Id));

    public async Task UpdateAsync(Models.User user) => await _usersContainer.UpsertItemAsync(
        user, new PartitionKey(user.Id));

    public async Task DeleteAsync(string id) => await _usersContainer.DeleteItemAsync<Models.User>(
        id, new PartitionKey(id));
}
