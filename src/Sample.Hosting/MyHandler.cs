class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Hello from MyHandler {message.DateSend}");
        return Task.FromResult(0);
    }
}