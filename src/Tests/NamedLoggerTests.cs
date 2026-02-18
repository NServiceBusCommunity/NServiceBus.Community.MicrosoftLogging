using NServiceBus.Logging;
using System.Threading.Tasks;

/// <summary>
/// Tests for the NamedLogger class that queues logs for deferred processing.
/// Uses the test implementation from DeferredLoggerFactoryTests.
/// </summary>
public class NamedLoggerTests
{
    [Test]
    public async Task Debug_writes_message_at_debug_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Debug("debug message");

        await Assert.That(factory.DeferredLogs["TestLogger"]).HasSingleItem();
        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(level).IsEqualTo(LogLevel.Debug);
        await Assert.That(message).IsEqualTo("debug message");
    }

    [Test]
    public async Task Debug_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("test exception");

        logger.Debug("debug message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).Contains("debug message");
        await Assert.That(message).Contains("test exception");
    }

    [Test]
    public async Task DebugFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.DebugFormat("value is {0}", 42);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).IsEqualTo("value is 42");
    }

    [Test]
    public async Task Info_writes_message_at_info_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Info("info message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(level).IsEqualTo(LogLevel.Info);
        await Assert.That(message).IsEqualTo("info message");
    }

    [Test]
    public async Task Info_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("info exception");

        logger.Info("info message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).Contains("info message");
        await Assert.That(message).Contains("info exception");
    }

    [Test]
    public async Task InfoFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.InfoFormat("user {0} logged in", "alice");

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).IsEqualTo("user alice logged in");
    }

    [Test]
    public async Task Warn_writes_message_at_warn_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Warn("warn message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(level).IsEqualTo(LogLevel.Warn);
        await Assert.That(message).IsEqualTo("warn message");
    }

    [Test]
    public async Task Warn_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("warn exception");

        logger.Warn("warn message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).Contains("warn message");
        await Assert.That(message).Contains("warn exception");
    }

    [Test]
    public async Task WarnFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.WarnFormat("warning: {0}", "issue");

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).IsEqualTo("warning: issue");
    }

    [Test]
    public async Task Error_writes_message_at_error_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Error("error message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(level).IsEqualTo(LogLevel.Error);
        await Assert.That(message).IsEqualTo("error message");
    }

    [Test]
    public async Task Error_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("error exception");

        logger.Error("error message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).Contains("error message");
        await Assert.That(message).Contains("error exception");
    }

    [Test]
    public async Task ErrorFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.ErrorFormat("error code: {0}", 500);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).IsEqualTo("error code: 500");
    }

    [Test]
    public async Task Fatal_writes_message_at_fatal_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Fatal("fatal message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(level).IsEqualTo(LogLevel.Fatal);
        await Assert.That(message).IsEqualTo("fatal message");
    }

    [Test]
    public async Task Fatal_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("fatal exception");

        logger.Fatal("fatal message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).Contains("fatal message");
        await Assert.That(message).Contains("fatal exception");
    }

    [Test]
    public async Task FatalFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.FatalFormat("fatal error in {0}", "module");

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        await Assert.That(message).IsEqualTo("fatal error in module");
    }

    [Test]
    public async Task Multiple_logs_preserve_order()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Info("first");
        logger.Info("second");
        logger.Info("third");

        var logs = factory.DeferredLogs["TestLogger"].ToList();
        await Assert.That(logs.Count).IsEqualTo(3);
        await Assert.That(logs[0].message).IsEqualTo("first");
        await Assert.That(logs[1].message).IsEqualTo("second");
        await Assert.That(logs[2].message).IsEqualTo("third");
    }

    [Test]
    public async Task IsDebugEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsDebugEnabled = true };

        await Assert.That(logger.IsDebugEnabled).IsTrue();

        logger.IsDebugEnabled = false;
        await Assert.That(logger.IsDebugEnabled).IsFalse();
    }

    [Test]
    public async Task IsInfoEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsInfoEnabled = true };

        await Assert.That(logger.IsInfoEnabled).IsTrue();
    }

    [Test]
    public async Task IsWarnEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsWarnEnabled = true };

        await Assert.That(logger.IsWarnEnabled).IsTrue();
    }

    [Test]
    public async Task IsErrorEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsErrorEnabled = true };

        await Assert.That(logger.IsErrorEnabled).IsTrue();
    }

    [Test]
    public async Task IsFatalEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsFatalEnabled = true };

        await Assert.That(logger.IsFatalEnabled).IsTrue();
    }
}