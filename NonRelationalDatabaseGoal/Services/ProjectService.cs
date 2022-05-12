using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class ProjectService : GenericService<Project>
{
    public ProjectService(CosmosClient client) : base(client.GetProjectsContainer())
    {
    }

    public async Task<IEnumerable<Project>> GetAsync(QueryStringParameters parameters) =>
        await Container.GetItemLinqQueryable<Project>()
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();
}
