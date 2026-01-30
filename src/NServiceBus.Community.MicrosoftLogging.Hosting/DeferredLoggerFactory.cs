using NServiceBus.Logging;

class DeferredLoggerFactory(LogLevel level) :
    ILoggerFactory
{
    public ConcurrentDictionary<string, ConcurrentQueue<(LogLevel level, string message)>> deferredLogs = [];

    public ILog GetLogger(Type type) =>
        GetLogger(type.FullName!);

    public ILog GetLogger(string name) =>
        new NamedLogger(name, this)
        {
            IsDebugEnabled = isDebugEnabled,
            IsInfoEnabled = isInfoEnabled,
            IsWarnEnabled = isWarnEnabled,
            IsErrorEnabled = isErrorEnabled,
            IsFatalEnabled = isFatalEnabled
        };

    public void Write(string name, LogLevel messageLevel, string message)
    {
        if (messageLevel < level)
        {
            return;
        }
        var logQueues = deferredLogs.GetOrAdd(name, new ConcurrentQueue<(LogLevel level, string message)>());
        logQueues.Enqueue((messageLevel, message));
    }

    bool isDebugEnabled = LogLevel.Debug >= level;
    bool isErrorEnabled = LogLevel.Error >= level;
    bool isFatalEnabled = LogLevel.Fatal >= level;
    bool isInfoEnabled = LogLevel.Info >= level;
    bool isWarnEnabled = LogLevel.Warn >= level;
}