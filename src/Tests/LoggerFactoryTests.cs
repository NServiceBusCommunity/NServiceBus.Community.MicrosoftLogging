using NServiceBus.Logging;
using System.Threading.Tasks;

/// <summary>
/// Tests for the LoggerFactory class through the public MicrosoftLogFactory API.
/// </summary>
public class LoggerFactoryTests
{
    [Test]
    public async Task GetLogger_with_type_returns_ILog_implementation()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        // Use LogManager to get a logger after configuring MicrosoftLogFactory
        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger(typeof(LoggerFactoryTests));

        await Assert.That(log).IsNotNull();
        await Assert.That(log).IsAssignableTo<ILog>();
    }

    [Test]
    public async Task GetLogger_with_string_returns_ILog_implementation()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger("MyCustomLogger");

        await Assert.That(log).IsNotNull();
        await Assert.That(log).IsAssignableTo<ILog>();
    }

    [Test]
    public async Task GetLogger_with_type_creates_logger_with_type_name()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        loggerFactory.GetLogger(typeof(LoggerFactoryTests));

        await Assert.That(mockFactory.Loggers).HasSingleItem();
        await Assert.That(mockFactory.Loggers.ContainsKey(typeof(LoggerFactoryTests).FullName!)).IsTrue();
    }

    [Test]
    public async Task GetLogger_with_string_creates_logger_with_given_name()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        loggerFactory.GetLogger("MyCustomLogger");

        await Assert.That(mockFactory.Loggers).HasSingleItem();
        await Assert.That(mockFactory.Loggers.ContainsKey("MyCustomLogger")).IsTrue();
    }

    [Test]
    public async Task GetLogger_with_type_logs_to_correct_logger()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger(typeof(LoggerFactoryTests));
        log.Info("test message");

        var mockLogger = mockFactory.Loggers[typeof(LoggerFactoryTests).FullName!];
        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test message");
    }

    [Test]
    public async Task GetLogger_with_string_logs_to_correct_logger()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger("CustomLogger");
        log.Warn("warning message");

        var mockLogger = mockFactory.Loggers["CustomLogger"];
        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("warning message");
    }

    [Test]
    public async Task GetLogger_called_multiple_times_with_same_type_returns_separate_wrapper_instances()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log1 = loggerFactory.GetLogger(typeof(LoggerFactoryTests));
        var log2 = loggerFactory.GetLogger(typeof(LoggerFactoryTests));

        await Assert.That(log2).IsNotSameReferenceAs(log1);
    }

    [Test]
    public async Task GetLogger_called_multiple_times_with_different_types_creates_different_loggers()
    {
        var mockFactory = new MockMsLoggerFactory();
        var factory = new MicrosoftLogFactory();
        factory.UseMsFactory(mockFactory);

        var loggerFactory = GetLoggingFactory(factory);
        loggerFactory.GetLogger(typeof(LoggerFactoryTests));
        loggerFactory.GetLogger(typeof(string));

        await Assert.That(mockFactory.Loggers.Count).IsEqualTo(2);
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