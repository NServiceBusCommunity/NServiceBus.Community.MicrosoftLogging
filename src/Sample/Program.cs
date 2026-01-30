using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus.Logging;

public static class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddLogging(logging => { logging.AddConsole(); });
        await using var provider = services.BuildServiceProvider();
        using var loggerFactory = provider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(loggerFactory);

        var configuration = new EndpointConfiguration("MicrosoftLoggingSample");
        configuration.UseTransport<LearningTransport>();
        var endpoint = await Endpoint.Start(configuration);
        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await endpoint.SendLocal(message);
        Console.WriteLine("Press any key to stop program");
        Console.Read();
        await endpoint.Stop();
    }
}