using NServiceBus.Logging;

/// <summary>
/// Tests for the NamedLogger class that queues logs for deferred processing.
/// Uses the test implementation from DeferredLoggerFactoryTests.
/// </summary>
public class NamedLoggerTests
{
    [Fact]
    public void Debug_writes_message_at_debug_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Debug("debug message");

        Assert.Single(factory.DeferredLogs["TestLogger"]);
        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal(LogLevel.Debug, level);
        Assert.Equal("debug message", message);
    }

    [Fact]
    public void Debug_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("test exception");

        logger.Debug("debug message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Contains("debug message", message);
        Assert.Contains("test exception", message);
    }

    [Fact]
    public void DebugFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.DebugFormat("value is {0}", 42);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal("value is 42", message);
    }

    [Fact]
    public void Info_writes_message_at_info_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Info("info message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal(LogLevel.Info, level);
        Assert.Equal("info message", message);
    }

    [Fact]
    public void Info_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("info exception");

        logger.Info("info message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Contains("info message", message);
        Assert.Contains("info exception", message);
    }

    [Fact]
    public void InfoFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.InfoFormat("user {0} logged in", "alice");

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal("user alice logged in", message);
    }

    [Fact]
    public void Warn_writes_message_at_warn_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Warn("warn message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal(LogLevel.Warn, level);
        Assert.Equal("warn message", message);
    }

    [Fact]
    public void Warn_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("warn exception");

        logger.Warn("warn message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Contains("warn message", message);
        Assert.Contains("warn exception", message);
    }

    [Fact]
    public void WarnFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.WarnFormat("warning: {0}", "issue");

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal("warning: issue", message);
    }

    [Fact]
    public void Error_writes_message_at_error_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Error("error message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal(LogLevel.Error, level);
        Assert.Equal("error message", message);
    }

    [Fact]
    public void Error_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("error exception");

        logger.Error("error message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Contains("error message", message);
        Assert.Contains("error exception", message);
    }

    [Fact]
    public void ErrorFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.ErrorFormat("error code: {0}", 500);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal("error code: 500", message);
    }

    [Fact]
    public void Fatal_writes_message_at_fatal_level()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Fatal("fatal message");

        var (level, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal(LogLevel.Fatal, level);
        Assert.Equal("fatal message", message);
    }

    [Fact]
    public void Fatal_with_exception_includes_exception_in_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);
        var exception = new InvalidOperationException("fatal exception");

        logger.Fatal("fatal message", exception);

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Contains("fatal message", message);
        Assert.Contains("fatal exception", message);
    }

    [Fact]
    public void FatalFormat_formats_message()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.FatalFormat("fatal error in {0}", "module");

        var (_, message) = factory.DeferredLogs["TestLogger"].First();
        Assert.Equal("fatal error in module", message);
    }

    [Fact]
    public void Multiple_logs_preserve_order()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory);

        logger.Info("first");
        logger.Info("second");
        logger.Info("third");

        var logs = factory.DeferredLogs["TestLogger"].ToList();
        Assert.Equal(3, logs.Count);
        Assert.Equal("first", logs[0].message);
        Assert.Equal("second", logs[1].message);
        Assert.Equal("third", logs[2].message);
    }

    [Fact]
    public void IsDebugEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsDebugEnabled = true };

        Assert.True(logger.IsDebugEnabled);

        logger.IsDebugEnabled = false;
        Assert.False(logger.IsDebugEnabled);
    }

    [Fact]
    public void IsInfoEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsInfoEnabled = true };

        Assert.True(logger.IsInfoEnabled);
    }

    [Fact]
    public void IsWarnEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsWarnEnabled = true };

        Assert.True(logger.IsWarnEnabled);
    }

    [Fact]
    public void IsErrorEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsErrorEnabled = true };

        Assert.True(logger.IsErrorEnabled);
    }

    [Fact]
    public void IsFatalEnabled_can_be_set()
    {
        var factory = new TestDeferredLoggerFactory(LogLevel.Debug);
        var logger = new TestNamedLogger("TestLogger", factory) { IsFatalEnabled = true };

        Assert.True(logger.IsFatalEnabled);
    }
}
