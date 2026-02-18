using Microsoft.Extensions.Logging;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;
using MsILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

class MockMsLogger : ILogger
{
    public List<LogEntry> LogEntries { get; } = [];
    public MsLogLevel EnabledLevel { get; set; } = MsLogLevel.Trace;

    public void Log<TState>(MsLogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var entry = new LogEntry
        {
            Level = logLevel,
            Message = formatter(state, exception),
            Exception = exception
        };

        if (state is IReadOnlyList<KeyValuePair<string, object?>> values)
        {
            var originalFormat = values.FirstOrDefault(v => v.Key == "{OriginalFormat}");
            if (originalFormat.Value is string format)
            {
                entry.Message = format;
                entry.Args = values
                    .Where(v => v.Key != "{OriginalFormat}")
                    .Select(v => v.Value!)
                    .ToArray();
            }
        }

        LogEntries.Add(entry);
    }

    public bool IsEnabled(MsLogLevel logLevel) => logLevel >= EnabledLevel;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}

class LogEntry
{
    public MsLogLevel Level { get; set; }
    public string Message { get; set; } = "";
    public Exception? Exception { get; set; }
    public object[]? Args { get; set; }
}

class MockMsLoggerFactory : MsILoggerFactory
{
    public Dictionary<string, MockMsLogger> Loggers { get; } = [];
    public MsLogLevel DefaultEnabledLevel { get; set; } = MsLogLevel.Trace;

    public ILogger CreateLogger(string categoryName)
    {
        if (!Loggers.TryGetValue(categoryName, out var logger))
        {
            logger = new()
            {
                EnabledLevel = DefaultEnabledLevel
            };
            Loggers[categoryName] = logger;
        }
        return logger;
    }

    public void AddProvider(ILoggerProvider provider)
    {
    }

    public void Dispose()
    {
    }
}
