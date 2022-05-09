using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Interfaces;

namespace NonRelationalDatabaseGoal.Services;

public class GenericService<T> where T : IIdentifiable
{
    protected Container Container { get; }

    public GenericService(Container container) => Container = container;

    public async Task<IEnumerable<T>> GetAllAsync() => await Container.GetAllAsync<T>();

    public async Task<T> GetByIdAsync(string id) => await Container.ReadItemAsync<T>(
        id, new PartitionKey(id));

    public async Task CreateAsync(T item) => await Container.CreateItemAsync(
        item, new PartitionKey(item.Id));

    public async Task UpdateAsync(T item) => await Container.UpsertItemAsync(
        item, new PartitionKey(item.Id));

    public async Task DeleteAsync(string id) => await Container.DeleteItemAsync<T>(
        id, new PartitionKey(id));
}
