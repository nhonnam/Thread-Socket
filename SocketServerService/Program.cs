using SocketServerService.Implementation;
using SocketServerService.Interfaces;

namespace SocketServerService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAuditLogger logger = new XmlAuditLogger();
            TcpServer server = new TcpServer(logger);

            server.Start();
        }
    }
}
