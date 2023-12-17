using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;

namespace SocketServerService
{
    internal class Program
    {
        private static readonly object AuditLock = new object();
        private static int clientCounter = 0;

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            Console.WriteLine("Server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clientCounter++;

                Thread clientThread = new Thread(() => HandleClient(clientCounter, client));
                clientThread.Start();
            }
        }

        private static void HandleClient(int clientId, TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string message = reader.ReadLine();

                    Console.WriteLine($"Received message from client {clientId}: {message}");

                    // Write to audit.xml
                    WriteToAuditXml(clientId, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client {clientId}: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private static void WriteToAuditXml(int clientId, string message)
        {
            lock (AuditLock)
            {
                XmlDocument xmlDoc = new XmlDocument();

                if (File.Exists("audit.xml"))
                {
                    xmlDoc.Load("audit.xml");
                }
                else
                {
                    XmlElement root = xmlDoc.CreateElement("AuditLog");
                    xmlDoc.AppendChild(root);
                }

                XmlElement messageElement = xmlDoc.CreateElement("Message");
                messageElement.SetAttribute("ThreadId", Thread.CurrentThread.ManagedThreadId.ToString());
                messageElement.SetAttribute("ClientId", clientId.ToString());
                messageElement.InnerText = message;

                xmlDoc.DocumentElement?.AppendChild(messageElement);
                xmlDoc.Save("audit.xml");
            }
        }
    }
}
