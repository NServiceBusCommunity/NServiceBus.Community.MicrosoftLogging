using Microsoft.Extensions.Hosting;
using MsLoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using NServiceBus.Logging;

class NServiceBusLoggingHostedService(MsLoggerFactory factory) :
    IHostedService
{
    public Task StartAsync(Cancel cancel)
    {
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(factory);

        foreach (var deferredLog in DeferredLoggerFactoryDefinition.Factory?.deferredLogs ??
            new ConcurrentDictionary<string, ConcurrentQueue<(LogLevel level, string message)>>())
        {
            var logger = LogManager.GetLogger(deferredLog.Key);
            foreach (var (level, message) in deferredLog.Value)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        logger.Debug(message);
                        break;
                    case LogLevel.Info:
                        logger.Info(message);
                        break;
                    case LogLevel.Warn:
                        logger.Warn(message);
                        break;
                    case LogLevel.Error:
                        logger.Error(message);
                        break;
                    case LogLevel.Fatal:
                        logger.Fatal(message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        DeferredLoggerFactoryDefinition.Factory?.deferredLogs.Clear();
        DeferredLoggerFactoryDefinition.Factory = null;

        return Task.CompletedTask;
    }

    public Task StopAsync(Cancel cancel) =>
        Task.CompletedTask;
}