using NServiceBus.Logging;
using MsLoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace NServiceBus;

/// <summary>
/// Configure NServiceBus logging messages to use Microsoft.Extensions.Logging.
/// Use by calling <see cref="LogManager.Use{T}"/> where the T is <see cref="MicrosoftLogFactory"/>.
/// </summary>
public class MicrosoftLogFactory :
    LoggingFactoryDefinition
{
    MsLoggerFactory? msLoggerFactory;

    /// <summary>
    /// <see cref="LoggingFactoryDefinition.GetLoggingFactory"/>.
    /// </summary>
    protected override ILoggerFactory GetLoggingFactory()
    {
        if (msLoggerFactory == null)
        {
            throw new($"Call {nameof(MicrosoftLogFactory)}.{nameof(UseMsFactory)}() prior to starting endpoint.");
        }
        return new LoggerFactory(msLoggerFactory);
    }

    /// <summary>
    /// Specifies the <see cref="MsLoggerFactory"/> to use when constructing <see cref="ILog"/> instances.
    /// </summary>
    public void UseMsFactory(MsLoggerFactory msLoggerFactory)
    {
        if (this.msLoggerFactory == null)
        {
            this.msLoggerFactory = msLoggerFactory;
            return;
        }
        throw new($"Call {nameof(UseMsFactory)} has already been called.");
    }
}