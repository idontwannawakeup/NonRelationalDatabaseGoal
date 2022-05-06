using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

static async Task<CosmosClient> InitializeCosmosClientAsync(string connectionString)
{
    var cosmosClient = new CosmosClient(connectionString);
    var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("NonRelationalDatabaseGoal");
    return cosmosClient;
}

builder.Services.AddSingleton<CosmosClient>(_ =>
{
    return InitializeCosmosClientAsync(builder.Configuration.GetConnectionString("DefaultConnection"))
        .GetAwaiter()
        .GetResult();
});

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
