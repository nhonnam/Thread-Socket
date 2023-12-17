using SocketClientService.Interfaces;
using System;
using System.Threading.Tasks;

namespace SocketClientService
{
    internal class UserMessageHandler
    {
        private readonly IMessageSender messageSender;

        public UserMessageHandler(IMessageSender sender)
        {
            messageSender = sender;
        }

        public void Start()
        {
            Console.Write("Enter your email address: ");
            string email = Console.ReadLine();

            if (!messageSender.IsValidEmail(email))
            {
                Console.WriteLine("Invalid email address. Exiting.");
                Task.Delay(1000).Wait();
                return;
            }

            Console.WriteLine("Enter your message (type 'quit' to exit):");

            while (true)
            {
                Console.Write("> ");
                string message = Console.ReadLine();

                if (message.ToLower() == "quit")
                {
                    break;
                }

                messageSender.SendMessage(email, message);
            }
        }
    }
}
