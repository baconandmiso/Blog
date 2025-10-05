using Aspire.Hosting;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres", port: 54194)
    .WithEnvironment("POSTGRES_DB", "blogdb")
    .WithDataVolume("VolumeMount.postgres.data")
    .WithPgAdmin(configureContainer: x => x.WithHostPort(54195))
    .AddDatabase("PostgreSqlConnection", "blogdb");

var api = builder.AddProject<WebApi>("WebApi")
        .WithReference(postgres);

var frontend = builder.AddNpmApp("Frontend", "../frontend", "dev")
    .WithHttpEndpoint(port: 8006, env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WithReference(postgres);

if (builder.Environment.IsDevelopment() && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see + https://github.com/dotnet/aspire/issues/3324 for more details.
    frontend.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder.Build().Run();
