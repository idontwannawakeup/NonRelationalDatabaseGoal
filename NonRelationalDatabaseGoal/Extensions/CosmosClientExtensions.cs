using Microsoft.Azure.Cosmos;

namespace NonRelationalDatabaseGoal.Extensions;

public static class CosmosClientExtensions
{
    public static Container GetUsersContainer(this CosmosClient client) => client.GetContainer(
        "NonRelationalDatabaseGoal",
        "Users");

    public static async Task<IEnumerable<T>> GetAllAsync<T>(this Container container)
    {
        using var iterator = container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
        return await iterator.ReadAllAsync();
    }

    public static async Task<IEnumerable<T>> ReadAllAsync<T>(this FeedIterator<T> iterator)
    {
        var feedResponses = new List<FeedResponse<T>>();
        while (iterator.HasMoreResults)
        {
            feedResponses.Add(await iterator.ReadNextAsync());
        }

        return feedResponses.SelectMany(item => item);
    }
}
