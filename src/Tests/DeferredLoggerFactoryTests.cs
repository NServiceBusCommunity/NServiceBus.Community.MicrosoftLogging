using NServiceBus.Logging;

/// <summary>
/// Tests for the DeferredLoggerFactory pattern using a local test implementation.
/// This tests the deferred logging pattern used for capturing logs before endpoint startup.
/// </summary>
public class DeferredLoggerFactoryTests
{
    [Test]
    public async Task GetLogger_with_type_returns_NamedLogger()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);

        var log = factory.GetLogger(typeof(DeferredLoggerFactoryTests));

        await Assert.That(log).IsNotNull();
        await Assert.That(log).IsTypeOf<TestNamedLogger>();
    }

    [Test]
    public async Task GetLogger_with_string_returns_NamedLogger()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);

        var log = factory.GetLogger("TestLogger");

        await Assert.That(log).IsNotNull();
        await Assert.That(log).IsTypeOf<TestNamedLogger>();
    }

    [Test]
    public async Task Write_adds_message_to_deferred_logs()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);

        factory.Write("TestLogger", LogLevel.Info, "test message");

        await Assert.That(factory.DeferredLogs).HasSingleItem();
        await Assert.That(factory.DeferredLogs.ContainsKey("TestLogger")).IsTrue();
        await Assert.That(factory.DeferredLogs["TestLogger"]).HasSingleItem();
        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(level).IsEqualTo(LogLevel.Info);
        await Assert.That(message).IsEqualTo("test message");
    }

    [Test]
    public async Task Write_filters_messages_below_configured_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Warn);

        factory.Write("TestLogger", LogLevel.Debug, "debug message");
        factory.Write("TestLogger", LogLevel.Info, "info message");

        await Assert.That(factory.DeferredLogs).IsEmpty();
    }

    [Test]
    public async Task Write_accepts_messages_at_or_above_configured_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Warn);

        factory.Write("TestLogger", LogLevel.Warn, "warn message");
        factory.Write("TestLogger", LogLevel.Error, "error message");
        factory.Write("TestLogger", LogLevel.Fatal, "fatal message");

        await Assert.That(factory.DeferredLogs).HasSingleItem();
        await Assert.That(factory.DeferredLogs["TestLogger"].Count).IsEqualTo(3);
    }

    [Test]
    public async Task Write_groups_messages_by_logger_name()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);

        factory.Write("Logger1", LogLevel.Info, "message 1");
        factory.Write("Logger2", LogLevel.Info, "message 2");
        factory.Write("Logger1", LogLevel.Info, "message 3");

        await Assert.That(factory.DeferredLogs.Count).IsEqualTo(2);
        await Assert.That(factory.DeferredLogs["Logger1"].Count).IsEqualTo(2);
        await Assert.That(factory.DeferredLogs["Logger2"]).HasSingleItem();
    }

    [Test]
    [Arguments(LogLevel.Debug, true, true, true, true, true)]
    [Arguments(LogLevel.Info, false, true, true, true, true)]
    [Arguments(LogLevel.Warn, false, false, true, true, true)]
    [Arguments(LogLevel.Error, false, false, false, true, true)]
    [Arguments(LogLevel.Fatal, false, false, false, false, true)]
    public async Task GetLogger_sets_enabled_levels_correctly(
        LogLevel configuredLevel,
        bool expectedDebug,
        bool expectedInfo,
        bool expectedWarn,
        bool expectedError,
        bool expectedFatal)
    {
        var factory = new TestDeferredLoggerFactory(configuredLevel);

        var log = factory.GetLogger("TestLogger");

        await Assert.That(log.IsDebugEnabled).IsEqualTo(expectedDebug);
        await Assert.That(log.IsInfoEnabled).IsEqualTo(expectedInfo);
        await Assert.That(log.IsWarnEnabled).IsEqualTo(expectedWarn);
        await Assert.That(log.IsErrorEnabled).IsEqualTo(expectedError);
        await Assert.That(log.IsFatalEnabled).IsEqualTo(expectedFatal);
    }

    [Test]
    public async Task Logger_writes_through_factory()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = factory.GetLogger("TestLogger");

        logger.Info("test info message");
        logger.Error("test error message");

        await Assert.That(factory.DeferredLogs).HasSingleItem();
        await Assert.That(factory.DeferredLogs["TestLogger"].Count).IsEqualTo(2);
    }
}

/// <summary>
/// Test implementation of DeferredLoggerFactory that mirrors the production code.
/// </summary>
class TestDeferredLoggerFactory(LogLevel level) :
    ILoggerFactory
{
    bool isDebugEnabled = LogLevel.Debug >= level;
    bool isInfoEnabled = LogLevel.Info >= level;
    bool isWarnEnabled = LogLevel.Warn >= level;
    bool isErrorEnabled = LogLevel.Error >= level;
    bool isFatalEnabled = LogLevel.Fatal >= level;

    public ConcurrentDictionary<string, ConcurrentQueue<(LogLevel level, string message)>> DeferredLogs { get; } = [];

    public ILog GetLogger(Type type) =>
        GetLogger(type.FullName!);

    public ILog GetLogger(string name) =>
        new TestNamedLogger(name, this)
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
        var logQueue = DeferredLogs.GetOrAdd(name, _ => new());
        logQueue.Enqueue((messageLevel, message));
    }
}

/// <summary>
/// Test implementation of NamedLogger that mirrors the production code.
/// </summary>
class TestNamedLogger(string name, TestDeferredLoggerFactory factory) :
    ILog
{
    public bool IsDebugEnabled { get; internal set; }
    public bool IsInfoEnabled { get; internal set; }
    public bool IsWarnEnabled { get; internal set; }
    public bool IsErrorEnabled { get; internal set; }
    public bool IsFatalEnabled { get; internal set; }

    public void Debug(string? message) =>
        factory.Write(name, LogLevel.Debug, message ?? "");

    public void Debug(string? message, Exception? exception) =>
        factory.Write(name, LogLevel.Debug, (message ?? "") + Environment.NewLine + exception);

    public void DebugFormat(string format, params object?[] args) =>
        factory.Write(name, LogLevel.Debug, string.Format(format, args));

    public void Info(string? message) =>
        factory.Write(name, LogLevel.Info, message ?? "");

    public void Info(string? message, Exception? exception) =>
        factory.Write(name, LogLevel.Info, (message ?? "") + Environment.NewLine + exception);

    public void InfoFormat(string format, params object?[] args) =>
        factory.Write(name, LogLevel.Info, string.Format(format, args));

    public void Warn(string? message) =>
        factory.Write(name, LogLevel.Warn, message ?? "");

    public void Warn(string? message, Exception? exception) =>
        factory.Write(name, LogLevel.Warn, (message ?? "") + Environment.NewLine + exception);

    public void WarnFormat(string format, params object?[] args) =>
        factory.Write(name, LogLevel.Warn, string.Format(format, args));

    public void Error(string? message) =>
        factory.Write(name, LogLevel.Error, message ?? "");

    public void Error(string? message, Exception? exception) =>
        factory.Write(name, LogLevel.Error, (message ?? "") + Environment.NewLine + exception);

    public void ErrorFormat(string format, params object?[] args) =>
        factory.Write(name, LogLevel.Error, string.Format(format, args));

    public void Fatal(string? message) =>
        factory.Write(name, LogLevel.Fatal, message ?? "");

    public void Fatal(string? message, Exception? exception) =>
        factory.Write(name, LogLevel.Fatal, (message ?? "") + Environment.NewLine + exception);

    public void FatalFormat(string format, params object?[] args) =>
        factory.Write(name, LogLevel.Fatal, string.Format(format, args));
}