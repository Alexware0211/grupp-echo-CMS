using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace Kvarter.Web.Data;

public class KvarterDbComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddUmbracoDbContext<KvarterDbContext>(
            (serviceProvider, options, connectionString, providerName) =>
            {
                if (string.IsNullOrEmpty(providerName) || string.IsNullOrEmpty(connectionString))
                {
                    return;
                }

                options.UseDatabaseProvider(providerName, connectionString);
            },
            shareUmbracoConnection: true);

        builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, RunKvarterMigrations>();
    }
}
