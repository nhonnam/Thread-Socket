using SocketServerService.Interfaces;
using System.IO;
using System.Threading;
using System.Xml;

namespace SocketServerService.Implementation
{
    internal class XmlAuditLogger : IAuditLogger
    {
        private readonly object auditLock = new object();

        public void LogMessage(int clientId, string message)
        {
            lock (auditLock)
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
