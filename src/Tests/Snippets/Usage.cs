using Microsoft.Extensions.Logging;
using NServiceBus.Logging;
using Microsoft.Extensions.DependencyInjection;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

class Usage
{
    Usage()
    {
        #region MsLoggingInCode

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFilter(level => level >= MsLogLevel.Information);
            loggingBuilder.AddConsole();
        });

        using var loggerFactory = new LoggerFactory();
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(loggerFactory);
        // endpoint startup and shutdown
        #endregion
    }
}