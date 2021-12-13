namespace AspStore.Dependencies
{
    public class MessageService: IMessageService
    {
        private int count = 0;
        public string GetGreetingMessage()
        {
            count++;
            return "Hello," + count;
        }
    }
}
