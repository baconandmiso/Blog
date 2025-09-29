using Aspire.Hosting;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<WebApi>("webapi");

var dbPassword = builder.AddParameter("dbpassword", "KinoemonLove");
var postgresDataPath = Path.Combine(builder.AppHostDirectory, "data", "postgres");
var postgres = builder.AddPostgres("postgres", port: 54194)
    .WithEnvironment("POSTGRES_DB", "blogdb")
    .WithDataVolume("VolumeMount.postgres.data")
    .WithPassword(dbPassword)
    .WithPgAdmin(configureContainer: x => x.WithHostPort(54195))
    .AddDatabase("blog", "blogdb");

var frontend = builder.AddNpmApp("frontend", "../frontend", "dev")
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
