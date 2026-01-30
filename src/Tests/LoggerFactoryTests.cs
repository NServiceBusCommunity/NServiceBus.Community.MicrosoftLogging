using NServiceBus.Logging;

/// <summary>
/// Tests for the LoggerFactory class through the public MicrosoftLogFactory API.
/// </summary>
public class LoggerFactoryTests
{
    [Fact]
    public void GetLogger_with_type_returns_ILog_implementation()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        // Use LogManager to get a logger after configuring MicrosoftLogFactory
        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger(typeof(LoggerFactoryTests));

        Assert.NotNull(log);
        Assert.IsAssignableFrom<ILog>(log);
    }

    [Fact]
    public void GetLogger_with_string_returns_ILog_implementation()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger("MyCustomLogger");

        Assert.NotNull(log);
        Assert.IsAssignableFrom<ILog>(log);
    }

    [Fact]
    public void GetLogger_with_type_creates_logger_with_type_name()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        loggerFactory.GetLogger(typeof(LoggerFactoryTests));

        Assert.Single(mockFactory.Loggers);
        Assert.True(mockFactory.Loggers.ContainsKey(typeof(LoggerFactoryTests).FullName!));
    }

    [Fact]
    public void GetLogger_with_string_creates_logger_with_given_name()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        loggerFactory.GetLogger("MyCustomLogger");

        Assert.Single(mockFactory.Loggers);
        Assert.True(mockFactory.Loggers.ContainsKey("MyCustomLogger"));
    }

    [Fact]
    public void GetLogger_with_type_logs_to_correct_logger()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger(typeof(LoggerFactoryTests));
        log.Info("test message");

        var mockLogger = mockFactory.Loggers[typeof(LoggerFactoryTests).FullName!];
        Assert.Single(mockLogger.LogEntries);
        Assert.Equal("test message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void GetLogger_with_string_logs_to_correct_logger()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger("CustomLogger");
        log.Warn("warning message");

        var mockLogger = mockFactory.Loggers["CustomLogger"];
        Assert.Single(mockLogger.LogEntries);
        Assert.Equal("warning message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void GetLogger_called_multiple_times_with_same_type_returns_separate_wrapper_instances()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log1 = loggerFactory.GetLogger(typeof(LoggerFactoryTests));
        var log2 = loggerFactory.GetLogger(typeof(LoggerFactoryTests));

        Assert.NotSame(log1, log2);
    }

    [Fact]
    public void GetLogger_called_multiple_times_with_different_types_creates_different_loggers()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        loggerFactory.GetLogger(typeof(LoggerFactoryTests));
        loggerFactory.GetLogger(typeof(string));

        Assert.Equal(2, mockFactory.Loggers.Count);
    }

    // Helper to invoke protected GetLoggingFactory method
    static ILoggerFactory GetLoggingFactory(MicrosoftLogFactory factory)
    {
        var method = typeof(MicrosoftLogFactory).GetMethod(
            "GetLoggingFactory",
            BindingFlags.NonPublic | BindingFlags.Instance);
        try
        {
            return (ILoggerFactory)method!.Invoke(factory, null)!;
        }
        catch (TargetInvocationException exception)
        {
            throw exception.InnerException!;
        }
    }
}
