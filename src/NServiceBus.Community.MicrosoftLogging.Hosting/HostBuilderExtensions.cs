using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.Logging;

/// <summary>
/// Extension methods to configure NServiceBus Logging for the .NET Core generic host.
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Configures the host to use <see cref="MicrosoftLogFactory"/> for NServiceBus logging.
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <param name="deferLogging">Intercepts all NServiceBus logging that is done before the endpoint is started and forwards them <see cref="MicrosoftLogFactory"/>.</param>
    /// <remarks>If <paramref name="deferLogging"/> is used this extension method needs to be called before any usage of NServiceBus such as <code>UseNServiceBus</code>.</remarks>
    public static IHostBuilder UseMicrosoftLogFactoryLogging(this IHostBuilder hostBuilder, bool deferLogging = true)
    {
        if (deferLogging)
        {
            LogManager.Use<DeferredLoggerFactoryDefinition>();
        }

        hostBuilder.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddHostedService<NServiceBusLoggingHostedService>();
        });

        return hostBuilder;
    }
}