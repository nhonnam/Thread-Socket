namespace SocketServerService.Interfaces
{
    internal interface IAuditLogger
    {
        void LogMessage(int clientId, string message);
    }
}
