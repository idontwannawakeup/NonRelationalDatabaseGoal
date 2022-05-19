using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Interfaces;
using NonRelationalDatabaseGoal.Interfaces.Services;
using NonRelationalDatabaseGoal.Seeding;
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

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<ITicketService, TicketService>();

builder.Services.AddTransient<ISeeder, Seeder>();

builder.Services.AddControllers().AddNewtonsoftJson();

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

if (args.Contains("--seed"))
{
    await app.Services.GetRequiredService<Seeder>().SeedDatabaseAsync();
}

app.Run();
