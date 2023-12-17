using SocketClientService.Interfaces;
using System;
using System.IO;
using System.Net.Sockets;

namespace SocketClientService.Implementation
{
    internal class TcpMessageSender : IMessageSender
    {
        public bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }

        public void SendMessage(string email, string message)
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
    }
}
