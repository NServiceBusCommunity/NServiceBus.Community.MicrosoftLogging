using NServiceBus.Logging;

class DeferredLoggerFactoryDefinition :
    LoggingFactoryDefinition
{
    public DeferredLoggerFactoryDefinition() =>
        level = new(() => LogLevel.Info);

    protected override ILoggerFactory GetLoggingFactory()
    {
        Factory = new(level.Value);
        return Factory;
    }

    public static DeferredLoggerFactory? Factory;

    Lazy<LogLevel> level;
}