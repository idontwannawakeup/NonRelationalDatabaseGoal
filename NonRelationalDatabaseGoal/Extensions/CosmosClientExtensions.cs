using Microsoft.Azure.Cosmos;

namespace NonRelationalDatabaseGoal.Extensions;

public static class CosmosClientExtensions
{
    public static Container GetUsersContainer(this CosmosClient client) => client.GetContainer(
        "NonRelationalDatabaseGoal",
        "Users");

    public static async IAsyncEnumerable<T> GetAll<T>(this Container container)
    {
        var query = container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
        while (query.HasMoreResults)
        {
            var set = await query.ReadNextAsync();
            foreach (var item in set)
            {
                yield return item;
            }
        }
    }
}
