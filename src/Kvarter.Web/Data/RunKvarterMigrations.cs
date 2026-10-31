using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace Kvarter.Web.Data;

public class RunKvarterMigrations : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly KvarterDbContext _dbContext;
    private readonly IRuntimeState _runtimeState;
    private readonly ILogger<RunKvarterMigrations> _logger;

    public RunKvarterMigrations(
        KvarterDbContext dbContext,
        IRuntimeState runtimeState,
        ILogger<RunKvarterMigrations> logger)
    {
        _dbContext = dbContext;
        _runtimeState = runtimeState;
        _logger = logger;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        // Wait until the installer has run; the tables are created on the next restart.
        if (_runtimeState.Level != RuntimeLevel.Run)
        {
            _logger.LogInformation("Skipping Kvarter migrations: Umbraco runtime level is {Level}.", _runtimeState.Level);
            return;
        }

        var pending = (await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
        if (pending.Count == 0)
        {
            return;
        }

        _logger.LogInformation("Applying Kvarter migrations: {Migrations}", string.Join(", ", pending));
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }
}
