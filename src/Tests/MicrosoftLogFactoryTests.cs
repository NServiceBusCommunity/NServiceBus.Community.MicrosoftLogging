public class MicrosoftLogFactoryTests
{
    [Test]
    public async Task UseMsFactory_sets_logger_factory()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory = new MockMsLoggerFactory();

        factory.UseMsFactory(msLoggerFactory);

        // Verify by getting the logging factory (which would throw if not set)
        var loggerFactory = GetLoggingFactory(factory);
        await Assert.That(loggerFactory).IsNotNull();
    }

    [Test]
    public async Task UseMsFactory_called_twice_throws_exception()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory1 = new MockMsLoggerFactory();
        var msLoggerFactory2 = new MockMsLoggerFactory();

        factory.UseMsFactory(msLoggerFactory1);

        var exception = Assert.Throws<Exception>(() => factory.UseMsFactory(msLoggerFactory2));
        await Assert.That(exception.Message).Contains("UseMsFactory");
        await Assert.That(exception.Message).Contains("already been called");
    }

    [Test]
    public async Task GetLoggingFactory_throws_when_UseMsFactory_not_called()
    {
        var factory = new MicrosoftLogFactory();

        var exception = Assert.Throws<Exception>(() => GetLoggingFactory(factory));
        await Assert.That(exception.Message).Contains("UseMsFactory");
        await Assert.That(exception.Message).Contains("prior to starting endpoint");
    }

    [Test]
    public async Task GetLoggingFactory_returns_ILoggerFactory_after_UseMsFactory_called()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory = new MockMsLoggerFactory();
        factory.UseMsFactory(msLoggerFactory);

        var loggerFactory = GetLoggingFactory(factory);

        await Assert.That(loggerFactory).IsNotNull();
        await Assert.That(loggerFactory).IsAssignableTo<NServiceBus.Logging.ILoggerFactory>();
    }

    [Test]
    public async Task GetLoggingFactory_returns_factory_that_creates_working_loggers()
    {
        var factory = new MicrosoftLogFactory();
        var msLoggerFactory = new MockMsLoggerFactory();
        factory.UseMsFactory(msLoggerFactory);

        var loggerFactory = GetLoggingFactory(factory);
        var log = loggerFactory.GetLogger("TestLogger");
        log.Info("test message");

        await Assert.That(msLoggerFactory.Loggers).HasSingleItem();
        await Assert.That(msLoggerFactory.Loggers["TestLogger"].LogEntries).HasSingleItem();
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