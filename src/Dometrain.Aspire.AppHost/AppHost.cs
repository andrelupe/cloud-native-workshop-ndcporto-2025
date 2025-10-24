
var builder = DistributedApplication.CreateBuilder(args);

var dbUsername = builder.AddParameter("postgres-username");
var dbPassword = builder.AddParameter("postgres-password", true);

var mainDb = builder.AddPostgres("main-db", dbUsername, dbPassword, port: 5433)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .AddDatabase("dometrain");

var cartDb = builder.AddAzureCosmosDB("cosmosdb")
    .RunAsPreviewEmulator(resourceBuilder =>
    {
        resourceBuilder.WithLifetime(ContainerLifetime.Persistent);
        resourceBuilder.WithDataVolume();
        resourceBuilder.WithDataExplorer();
    })
    .AddCosmosDatabase("cartdb")
    .AddContainer("carts", "/pk");

builder.AddProject<Projects.Dometrain_Monolith_Api>("dometrain-api")
    .WithReference(mainDb).WaitFor(mainDb)
    .WithReference(cartDb).WaitFor(cartDb);

var app = builder.Build();
    
app.Run();
