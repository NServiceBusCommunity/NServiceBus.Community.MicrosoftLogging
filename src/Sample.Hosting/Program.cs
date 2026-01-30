using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public static class Program
{
    public static async Task Main()
    {
        using var host = CreateHostBuilder().Build();
        await host.StartAsync();

        var messageSession = host.Services.GetService<IMessageSession>();

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await messageSession.SendLocal(message);
        Console.WriteLine("Press any key to stop program");

        Console.Read();
        await host.StopAsync();
    }

    static IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
        // should go before UseNServiceBus
        builder.UseMicrosoftLogFactoryLogging();
        builder.UseNServiceBus(_ =>
        {
            var configuration = new EndpointConfiguration("MicrosoftLoggingSample");
            configuration.UseTransport<LearningTransport>();
            return configuration;
        });
        return builder;
    }
}