namespace SocketClientService.Interfaces
{
    internal interface IMessageSender
    {
        void SendMessage(string email, string message);
        bool IsValidEmail(string email);
    }
}
