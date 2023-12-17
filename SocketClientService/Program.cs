using System;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

namespace SocketClientService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your email address: ");
            string email = Console.ReadLine();

            // Validate email (simple validation for illustration)
            if (!IsValidEmail(email))
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

                SendMessage(email, message);
            }
        }

        private static void SendMessage(string email, string message)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 8888))
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string fullMessage = $"[{DateTime.Now}] {email}: {message}";
                writer.WriteLine(fullMessage);
                writer.Flush();
            }
        }

        private static bool IsValidEmail(string email)
        {
            // Simple email validation for illustration
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }
    }
}
