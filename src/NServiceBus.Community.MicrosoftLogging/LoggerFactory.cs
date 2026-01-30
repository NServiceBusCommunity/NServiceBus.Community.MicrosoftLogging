using Microsoft.Extensions.Logging;
using NServiceBus.Logging;
using ILoggerFactory = NServiceBus.Logging.ILoggerFactory;
using MsLoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

class LoggerFactory :
    ILoggerFactory
{
    MsLoggerFactory msFactory;

    public LoggerFactory(MsLoggerFactory msFactory) =>
        this.msFactory = msFactory;

    public ILog GetLogger(Type type)
    {
        var logger = msFactory.CreateLogger(type);
        return new Logger(logger);
    }

    public ILog GetLogger(string name)
    {
        var logger = msFactory.CreateLogger(name);
        return new Logger(logger);
    }
}