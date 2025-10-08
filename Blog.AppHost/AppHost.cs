using Aspire.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}

var pgPassword = builder.AddParameter("pgPassword");

var postgres = builder.AddPostgres("postgres", password: pgPassword, port: 54194)
    .WithEnvironment("POSTGRES_DB", "testdb")
    .WithDataVolume("VolumeMount.postgres.data")
    .WithPgAdmin(configureContainer: x => x.WithHostPort(54195))
    .AddDatabase("blogdb", "testdb");

var migrations = builder.AddProject<Blog_MigrationService>("migrations")
    .WithReference(postgres)
    .WaitFor(postgres);

var webapi = builder.AddProject<Blog_WebApi>("webapi")
        .WithReference(postgres)
        .WithReference(migrations)
        .WaitForCompletion(migrations);

var frontend = builder.AddNpmApp("frontend", "../frontend", "dev")
    .WithHttpEndpoint(port: 8006, env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(webapi)
    .WithReference(postgres);

if (builder.Environment.IsDevelopment() && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see + https://github.com/dotnet/aspire/issues/3324 for more details.
    frontend.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder.Build().Run();
