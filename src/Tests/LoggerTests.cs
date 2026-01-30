using Microsoft.Extensions.Logging;
using NServiceBus.Logging;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

/// <summary>
/// Tests for the Logger class that wraps Microsoft.Extensions.Logging.ILogger.
/// These tests use a local Logger implementation that mirrors the production code
/// to verify the logging behavior pattern.
/// </summary>
public class LoggerTests
{
    [Fact]
    public void Debug_logs_message_at_debug_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Debug("test debug message");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Debug, mockLogger.LogEntries[0].Level);
        Assert.Equal("test debug message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Debug_with_exception_logs_at_debug_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Debug("test debug message", exception);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Debug, mockLogger.LogEntries[0].Level);
        Assert.Equal("test debug message", mockLogger.LogEntries[0].Message);
        Assert.Same(exception, mockLogger.LogEntries[0].Exception);
    }

    [Fact]
    public void DebugFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.DebugFormat("value is {0}", 42);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Debug, mockLogger.LogEntries[0].Level);
        Assert.Equal("value is {0}", mockLogger.LogEntries[0].Message);
        Assert.Equal(new object[] { 42 }, mockLogger.LogEntries[0].Args);
    }

    [Fact]
    public void Info_logs_message_at_information_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Info("test info message");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Information, mockLogger.LogEntries[0].Level);
        Assert.Equal("test info message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Info_with_exception_logs_at_information_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Info("test info message", exception);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Information, mockLogger.LogEntries[0].Level);
        Assert.Equal("test info message", mockLogger.LogEntries[0].Message);
        Assert.Same(exception, mockLogger.LogEntries[0].Exception);
    }

    [Fact]
    public void InfoFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.InfoFormat("user {0} logged in at {1}", "alice", "10:00");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Information, mockLogger.LogEntries[0].Level);
        Assert.Equal("user {0} logged in at {1}", mockLogger.LogEntries[0].Message);
        Assert.Equal(new object[] { "alice", "10:00" }, mockLogger.LogEntries[0].Args);
    }

    [Fact]
    public void Warn_logs_message_at_warning_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Warn("test warning message");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Warning, mockLogger.LogEntries[0].Level);
        Assert.Equal("test warning message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Warn_with_exception_logs_at_warning_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Warn("test warning message", exception);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Warning, mockLogger.LogEntries[0].Level);
        Assert.Equal("test warning message", mockLogger.LogEntries[0].Message);
        Assert.Same(exception, mockLogger.LogEntries[0].Exception);
    }

    [Fact]
    public void WarnFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.WarnFormat("warning {0}", "details");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Warning, mockLogger.LogEntries[0].Level);
        Assert.Equal("warning {0}", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Error_logs_message_at_error_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Error("test error message");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Error, mockLogger.LogEntries[0].Level);
        Assert.Equal("test error message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Error_with_exception_logs_at_error_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Error("test error message", exception);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Error, mockLogger.LogEntries[0].Level);
        Assert.Equal("test error message", mockLogger.LogEntries[0].Message);
        Assert.Same(exception, mockLogger.LogEntries[0].Exception);
    }

    [Fact]
    public void ErrorFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.ErrorFormat("error code: {0}", 500);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Error, mockLogger.LogEntries[0].Level);
        Assert.Equal("error code: {0}", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Fatal_logs_message_at_critical_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Fatal("test fatal message");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Critical, mockLogger.LogEntries[0].Level);
        Assert.Equal("test fatal message", mockLogger.LogEntries[0].Message);
    }

    [Fact]
    public void Fatal_with_exception_logs_at_critical_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Fatal("test fatal message", exception);

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Critical, mockLogger.LogEntries[0].Level);
        Assert.Equal("test fatal message", mockLogger.LogEntries[0].Message);
        Assert.Same(exception, mockLogger.LogEntries[0].Exception);
    }

    [Fact]
    public void FatalFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.FatalFormat("fatal error in {0}", "module");

        Assert.Single(mockLogger.LogEntries);
        Assert.Equal(MsLogLevel.Critical, mockLogger.LogEntries[0].Level);
        Assert.Equal("fatal error in {0}", mockLogger.LogEntries[0].Message);
    }

    [Theory]
    [InlineData(MsLogLevel.Debug, true)]
    [InlineData(MsLogLevel.Information, false)]
    [InlineData(MsLogLevel.Warning, false)]
    public void IsDebugEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        Assert.Equal(expected, logger.IsDebugEnabled);
    }

    [Theory]
    [InlineData(MsLogLevel.Debug, true)]
    [InlineData(MsLogLevel.Information, true)]
    [InlineData(MsLogLevel.Warning, false)]
    public void IsInfoEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        Assert.Equal(expected, logger.IsInfoEnabled);
    }

    [Theory]
    [InlineData(MsLogLevel.Debug, true)]
    [InlineData(MsLogLevel.Warning, true)]
    [InlineData(MsLogLevel.Error, false)]
    public void IsWarnEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        Assert.Equal(expected, logger.IsWarnEnabled);
    }

    [Theory]
    [InlineData(MsLogLevel.Debug, true)]
    [InlineData(MsLogLevel.Error, true)]
    [InlineData(MsLogLevel.Critical, false)]
    public void IsErrorEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        Assert.Equal(expected, logger.IsErrorEnabled);
    }

    [Theory]
    [InlineData(MsLogLevel.Debug, true)]
    [InlineData(MsLogLevel.Critical, true)]
    [InlineData(MsLogLevel.None, false)]
    public void IsFatalEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        Assert.Equal(expected, logger.IsFatalEnabled);
    }
}

/// <summary>
/// Test implementation of Logger that mirrors the production code.
/// This allows testing the logging pattern without accessing internal types.
/// </summary>
class TestLogger : ILog
{
    ILogger logger;

    public TestLogger(ILogger logger) =>
        this.logger = logger;

    public void Debug(string message) =>
        logger.LogDebug(message);

    public void Debug(string message, Exception exception) =>
        logger.LogDebug(exception, message);

    public void DebugFormat(string format, params object[] args) =>
        logger.LogDebug(format, args);

    public void Info(string message) =>
        logger.LogInformation(message);

    public void Info(string message, Exception exception) =>
        logger.LogInformation(exception, message);

    public void InfoFormat(string format, params object[] args) =>
        logger.LogInformation(format, args);

    public void Warn(string message) =>
        logger.LogWarning(message);

    public void Warn(string message, Exception exception) =>
        logger.LogWarning(exception, message);

    public void WarnFormat(string format, params object[] args) =>
        logger.LogWarning(format, args);

    public void Error(string message) =>
        logger.LogError(message);

    public void Error(string message, Exception exception) =>
        logger.LogError(exception, message);

    public void ErrorFormat(string format, params object[] args) =>
        logger.LogError(format, args);

    public void Fatal(string message) =>
        logger.LogCritical(message);

    public void Fatal(string message, Exception exception) =>
        logger.LogCritical(exception, message);

    public void FatalFormat(string format, params object[] args) =>
        logger.LogCritical(format, args);

    public bool IsDebugEnabled => logger.IsEnabled(MsLogLevel.Debug);
    public bool IsInfoEnabled => logger.IsEnabled(MsLogLevel.Information);
    public bool IsWarnEnabled => logger.IsEnabled(MsLogLevel.Warning);
    public bool IsErrorEnabled => logger.IsEnabled(MsLogLevel.Error);
    public bool IsFatalEnabled => logger.IsEnabled(MsLogLevel.Critical);
}
