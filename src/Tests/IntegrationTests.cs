using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class IntegrationTests
{
    [Test]
    public async Task Ensure_log_messages_are_redirected()
    {
        var msLoggerFactory = new LoggerFactory();
        var logMessageCapture = new LogMessageCapture();
        msLoggerFactory.AddProvider(logMessageCapture);
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(msLoggerFactory);

        var configuration = new EndpointConfiguration("Tests");
        configuration.UseTransport<LearningTransport>();
        configuration.UseSerialization<SystemJsonSerializer>();
        var cancel = TestContext.Current.CancellationToken;
        var endpoint = await Endpoint.Start(configuration, cancel);

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await endpoint.SendLocal(message, cancellationToken: cancel);
        await Task.Delay(500, cancel);
        await Assert.That(LogMessageCapture.LoggingEvents).IsNotEmpty();
        await endpoint.Stop(cancel);
    }

    [Test]
    public Task Ensure_log_messages_are_redirected_in_Hosting_deferred() =>
        RunWithHost(true);

    [Test]
    public Task Ensure_log_messages_are_redirected_in_Hosting_not_deferred() =>
        RunWithHost(false);

    static async Task RunWithHost(bool deferLogging)
    {
        // Note: NServiceBus.Extensions.Hosting automatically integrates with
        // Microsoft.Extensions.Logging, so UseMicrosoftLogFactoryLogging is not needed.
        // The deferLogging parameter is kept for API compatibility but is handled
        // automatically by the hosting package.
        _ = deferLogging; // Suppress unused parameter warning

        var builder = Host.CreateDefaultBuilder();
        var logMessageCapture = new LogMessageCapture();
        builder.ConfigureLogging(logging =>
        {
            // Clear default providers (including EventLog on Windows) to prevent
            // disposed logger issues when running tests sequentially
            logging.ClearProviders();
            logging.AddProvider(logMessageCapture);
        });
        builder.UseNServiceBus(_ =>
        {
            var configuration = new EndpointConfiguration("HostingTest");
            configuration.UseTransport<LearningTransport>();
            configuration.UseSerialization<SystemJsonSerializer>();
            return configuration;
        });

        using var host = builder.Build();
        await host.StartAsync();
        var messageSession = host.Services.GetService<IMessageSession>();

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await messageSession.SendLocal(message);
        await Task.Delay(500);

        await Assert.That(LogMessageCapture.LoggingEvents).IsNotEmpty();
        await host.StopAsync();
    }

    public IntegrationTests() =>
        LogMessageCapture.LoggingEvents.Clear();
}