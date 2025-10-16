var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var sql = builder.AddSqlServer("sqldb").WithDataVolume().WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("myapp");

var keycloak = builder
    .AddKeycloak("keycloak", 8090)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRealmImport("./Realms");

var apiService = builder
    .AddProject<Projects.Aspire_ApiService>("apiservice")
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WaitFor(database)
    .WithReference(database, "MyAppDb");

builder
    .AddProject<Projects.Aspire_WebApp>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WithReference(keycloak)
    .WaitFor(apiService)
    .WaitFor(keycloak);

builder.Build().Run();
