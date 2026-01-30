public class MicrosoftLogFactoryTests
{
    [Fact]
    public void UseMsFactory_sets_logger_factory()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory = new MockMsLoggerFactory();

        factory.UseMsFactory(msLoggerFactory);

        // Verify by getting the logging factory (which would throw if not set)
        var loggerFactory = GetLoggingFactory(factory);
        Assert.NotNull(loggerFactory);
    }

    [Fact]
    public void UseMsFactory_called_twice_throws_exception()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory1 = new MockMsLoggerFactory();
        var msLoggerFactory2 = new MockMsLoggerFactory();

        factory.UseMsFactory(msLoggerFactory1);

        var exception = Assert.Throws<Exception>(() => factory.UseMsFactory(msLoggerFactory2));
        Assert.Contains("UseMsFactory", exception.Message);
        Assert.Contains("already been called", exception.Message);
    }

    [Fact]
    public void GetLoggingFactory_throws_when_UseMsFactory_not_called()
    {
        var factory = new MicrosoftLogFactory();

        var exception = Assert.Throws<Exception>(() => GetLoggingFactory(factory));
        Assert.Contains("UseMsFactory", exception.Message);
        Assert.Contains("prior to starting endpoint", exception.Message);
    }

    [Fact]
    public void GetLoggingFactory_returns_ILoggerFactory_after_UseMsFactory_called()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory = new MockMsLoggerFactory();
        factory.UseMsFactory(msLoggerFactory);

        var loggerFactory = GetLoggingFactory(factory);

        Assert.NotNull(loggerFactory);
        Assert.IsAssignableFrom<NServiceBus.Logging.ILoggerFactory>(loggerFactory);
    }

    [Fact]
    public void GetLoggingFactory_returns_factory_that_creates_working_loggers()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory = new MockMsLoggerFactory();
        factory.UseMsFactory(msLoggerFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger("TestLogger");
        log.Info("test message");

        Assert.Single(msLoggerFactory.Loggers);
        Assert.Single(msLoggerFactory.Loggers["TestLogger"].LogEntries);
    }

    // Helper to invoke protected GetLoggingFactory method
    static NServiceBus.Logging.ILoggerFactory GetLoggingFactory(MicrosoftLogFactory factory)
    {
        var method = typeof(MicrosoftLogFactory).GetMethod(
            "GetLoggingFactory",
            BindingFlags.NonPublic | BindingFlags.Instance);
        try
        {
            return (NServiceBus.Logging.ILoggerFactory)method!.Invoke(factory, null)!;
        }
        catch (TargetInvocationException exception)
        {
            throw exception.InnerException!;
        }
    }
}
