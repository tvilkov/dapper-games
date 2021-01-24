using System;
using DapperGames.Application.Interfaces;
using DapperGames.Application.Interfaces.Notification;

namespace DapperGames.Infrastructure
{
    internal class EmailSender : IEmailSender
    {
        public void SendEmail(string toAddress, string subject, string body)
        {
            Console.WriteLine($"Sending email to {toAddress} subj: {subject}");
        }
    }
}