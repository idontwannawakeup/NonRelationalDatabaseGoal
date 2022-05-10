using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Services;

var builder = WebApplication.CreateBuilder(args);

static async Task<CosmosClient> InitializeCosmosClientAsync(string connectionString)
{
    var cosmosClient = new CosmosClient(connectionString);
    var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("NonRelationalDatabaseGoal");
    await databaseResponse.Database.CreateContainerIfNotExistsAsync("Users", "/id");
    await databaseResponse.Database.CreateContainerIfNotExistsAsync("Teams", "/id");
    await databaseResponse.Database.CreateContainerIfNotExistsAsync("Projects", "/id");
    await databaseResponse.Database.CreateContainerIfNotExistsAsync("Tickets", "/id");
    return cosmosClient;
}

builder.Services.AddSingleton<CosmosClient>(_ =>
{
    return InitializeCosmosClientAsync(builder.Configuration.GetConnectionString("DefaultConnection"))
        .GetAwaiter()
        .GetResult();
});

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<TeamService>();
builder.Services.AddTransient<ProjectService>();
builder.Services.AddTransient<TicketService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
