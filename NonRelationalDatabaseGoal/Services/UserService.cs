using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;

namespace NonRelationalDatabaseGoal.Services;

public class UserService
{
    private readonly Container _usersContainer;

    public UserService(CosmosClient client) => _usersContainer = client.GetUsersContainer();

    public IAsyncEnumerable<User> GetAll() => _usersContainer.GetAll<User>();

    public async Task<User> GetByIdAsync(string id) => await _usersContainer.ReadItemAsync<User>(
        id, new PartitionKey(id));

    public async Task CreateAsync(User user) => await _usersContainer.CreateItemAsync<User>(
        user, new PartitionKey(user.Id));

    public async Task UpdateAsync(User user) => await _usersContainer.UpsertItemAsync<User>(
        user, new PartitionKey(user.Id));

    public async Task DeleteAsync(string id) => await _usersContainer.DeleteItemAsync<User>(
        id, new PartitionKey(id));
}
