namespace DapperGames.Application.Interfaces.Notification
{
    public interface IEmailSender
    {
        void SendEmail(string toAddress, string subject, string body);
    }
}