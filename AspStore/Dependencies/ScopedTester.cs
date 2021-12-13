namespace AspStore.Dependencies
{
    public class ScopedTester
    {
        private readonly IMessageService _messageService;
        public ScopedTester(IMessageService message)
        {
            message.GetGreetingMessage();
            _messageService = message;
        }
    }
}
