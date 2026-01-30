using NServiceBus.Logging;

class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        LogManager.GetLogger<MyHandler>().Info($"Hello from MyHandler {message.DateSend}");
        return Task.FromResult(0);
    }
}