using SocketClientService.Implementation;
using SocketClientService.Interfaces;

namespace SocketClientService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IMessageSender messageSender = new TcpMessageSender();
            UserMessageHandler messageHandler = new UserMessageHandler(messageSender);

            messageHandler.Start();
        }
    }
}
