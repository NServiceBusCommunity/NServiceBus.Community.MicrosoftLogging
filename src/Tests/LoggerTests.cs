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
    [Test]
    public async Task Debug_logs_message_at_debug_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Debug("test debug message");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Debug);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test debug message");
    }

    [Test]
    public async Task Debug_with_exception_logs_at_debug_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Debug("test debug message", exception);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Debug);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test debug message");
        await Assert.That(mockLogger.LogEntries[0].Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task DebugFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.DebugFormat("value is {0}", 42);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Debug);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("value is {0}");
        await Assert.That(mockLogger.LogEntries[0].Args).IsEqualTo([42]);
    }

    [Test]
    public async Task Info_logs_message_at_information_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Info("test info message");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Information);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test info message");
    }

    [Test]
    public async Task Info_with_exception_logs_at_information_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Info("test info message", exception);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Information);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test info message");
        await Assert.That(mockLogger.LogEntries[0].Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task InfoFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.InfoFormat("user {0} logged in at {1}", "alice", "10:00");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Information);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("user {0} logged in at {1}");
        await Assert.That(mockLogger.LogEntries[0].Args).IsEqualTo(["alice", "10:00"]);
    }

    [Test]
    public async Task Warn_logs_message_at_warning_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Warn("test warning message");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Warning);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test warning message");
    }

    [Test]
    public async Task Warn_with_exception_logs_at_warning_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Warn("test warning message", exception);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Warning);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test warning message");
        await Assert.That(mockLogger.LogEntries[0].Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task WarnFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.WarnFormat("warning {0}", "details");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Warning);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("warning {0}");
    }

    [Test]
    public async Task Error_logs_message_at_error_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Error("test error message");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Error);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test error message");
    }

    [Test]
    public async Task Error_with_exception_logs_at_error_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Error("test error message", exception);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Error);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test error message");
        await Assert.That(mockLogger.LogEntries[0].Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task ErrorFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.ErrorFormat("error code: {0}", 500);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Error);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("error code: {0}");
    }

    [Test]
    public async Task Fatal_logs_message_at_critical_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.Fatal("test fatal message");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Critical);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test fatal message");
    }

    [Test]
    public async Task Fatal_with_exception_logs_at_critical_level()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);
        var exception = new InvalidOperationException("test exception");

        logger.Fatal("test fatal message", exception);

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Critical);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("test fatal message");
        await Assert.That(mockLogger.LogEntries[0].Exception).IsSameReferenceAs(exception);
    }

    [Test]
    public async Task FatalFormat_formats_message()
    {
        var mockLogger = new MockMsLogger();
        var logger = new TestLogger(mockLogger);

        logger.FatalFormat("fatal error in {0}", "module");

        await Assert.That(mockLogger.LogEntries).HasSingleItem();
        await Assert.That(mockLogger.LogEntries[0].Level).IsEqualTo(MsLogLevel.Critical);
        await Assert.That(mockLogger.LogEntries[0].Message).IsEqualTo("fatal error in {0}");
    }

    [Test]
    [Arguments(MsLogLevel.Debug, true)]
    [Arguments(MsLogLevel.Information, false)]
    [Arguments(MsLogLevel.Warning, false)]
    public async Task IsDebugEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        await Assert.That(logger.IsDebugEnabled).IsEqualTo(expected);
    }

    [Test]
    [Arguments(MsLogLevel.Debug, true)]
    [Arguments(MsLogLevel.Information, true)]
    [Arguments(MsLogLevel.Warning, false)]
    public async Task IsInfoEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        await Assert.That(logger.IsInfoEnabled).IsEqualTo(expected);
    }

    [Test]
    [Arguments(MsLogLevel.Debug, true)]
    [Arguments(MsLogLevel.Warning, true)]
    [Arguments(MsLogLevel.Error, false)]
    public async Task IsWarnEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        await Assert.That(logger.IsWarnEnabled).IsEqualTo(expected);
    }

    [Test]
    [Arguments(MsLogLevel.Debug, true)]
    [Arguments(MsLogLevel.Error, true)]
    [Arguments(MsLogLevel.Critical, false)]
    public async Task IsErrorEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        await Assert.That(logger.IsErrorEnabled).IsEqualTo(expected);
    }

    [Test]
    [Arguments(MsLogLevel.Debug, true)]
    [Arguments(MsLogLevel.Critical, true)]
    [Arguments(MsLogLevel.None, false)]
    public async Task IsFatalEnabled_returns_correct_value(MsLogLevel enabledLevel, bool expected)
    {
        var mockLogger = new MockMsLogger { EnabledLevel = enabledLevel };
        var logger = new TestLogger(mockLogger);

        await Assert.That(logger.IsFatalEnabled).IsEqualTo(expected);
    }
}

/// <summary>
/// Test implementation of Logger that mirrors the production code.
/// This allows testing the logging pattern without accessing internal types.
/// </summary>
class TestLogger(ILogger logger) :
    ILog
{
    public void Debug(string? message) =>
        logger.LogDebug(message);

    public void Debug(string? message, Exception? exception) =>
        logger.LogDebug(exception, message);

    public void DebugFormat(string format, params object?[] args) =>
        logger.LogDebug(format, args);

    public void Info(string? message) =>
        logger.LogInformation(message);

    public void Info(string? message, Exception? exception) =>
        logger.LogInformation(exception, message);

    public void InfoFormat(string format, params object?[] args) =>
        logger.LogInformation(format, args);

    public void Warn(string? message) =>
        logger.LogWarning(message);

    public void Warn(string? message, Exception? exception) =>
        logger.LogWarning(exception, message);

    public void WarnFormat(string format, params object?[] args) =>
        logger.LogWarning(format, args);

    public void Error(string? message) =>
        logger.LogError(message);

    public void Error(string? message, Exception? exception) =>
        logger.LogError(exception, message);

    public void ErrorFormat(string format, params object?[] args) =>
        logger.LogError(format, args);

    public void Fatal(string? message) =>
        logger.LogCritical(message);

    public void Fatal(string? message, Exception? exception) =>
        logger.LogCritical(exception, message);

    public void FatalFormat(string format, params object?[] args) =>
        logger.LogCritical(format, args);

    public bool IsDebugEnabled => logger.IsEnabled(MsLogLevel.Debug);
    public bool IsInfoEnabled => logger.IsEnabled(MsLogLevel.Information);
    public bool IsWarnEnabled => logger.IsEnabled(MsLogLevel.Warning);
    public bool IsErrorEnabled => logger.IsEnabled(MsLogLevel.Error);
    public bool IsFatalEnabled => logger.IsEnabled(MsLogLevel.Critical);
}