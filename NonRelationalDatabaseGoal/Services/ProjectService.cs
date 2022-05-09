using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Services;

public class ProjectService : GenericService<Project>
{
    public ProjectService(CosmosClient client) : base(client.GetProjectsContainer())
    {
    }
}
