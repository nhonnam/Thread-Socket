using SocketServerService.Interfaces;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServerService
{
    internal class TcpServer
    {
        private readonly TcpListener server;
        private readonly IAuditLogger auditLogger;

        public TcpServer(IAuditLogger logger)
        {
            server = new TcpListener(IPAddress.Any, 8888);
            auditLogger = logger;
        }

        public void Start()
        {
            server.Start();
            Console.WriteLine("Server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string message = reader.ReadLine();

                    Console.WriteLine($"Received message from thread {threadId}: {message}");

                    auditLogger.LogMessage(threadId, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client {threadId}: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
