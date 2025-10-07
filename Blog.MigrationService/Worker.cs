using System.Diagnostics;
using Blog.Entity;
using Blog.Repository;
using Microsoft.EntityFrameworkCore;

namespace Blog.MigrationService;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public const string ActivitySourceName = "Migrations";

    private static readonly ActivitySource s_activitySource = new ActivitySource(ActivitySourceName);

    public Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating Database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await MigrationAsync(dbContext, stoppingToken);
            await AdminUserSeedAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    private static async Task MigrationAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task AdminUserSeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var adminUser = new AdminUser
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin")
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.AdminUsers.AddAsync(adminUser, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}