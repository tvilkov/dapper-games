using System;
using DapperGames.Application.Interfaces;
using DapperGames.Application.Interfaces.Common;
using DapperGames.Application.Interfaces.Cqrs;
using DapperGames.Application.Interfaces.Notification;
using DapperGames.Application.Interfaces.Persistence;

namespace DapperGames.Application.Commands
{
    public class ChangeEmailCommand : ICommand
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }
        public int ConfirmationTokenId { get; set; }
        public string ConfirmationToken { get; set; }
    }

    public class ChangeEmailCommandHandler : ICommandHandler<ChangeEmailCommand>
    {
        private readonly IClock _clock;
        private readonly ITransactionManager _tranManager;
        private readonly IUserRepository _users;
        private readonly ITokenRepository _tokens;
        private readonly IEmailSender _emailSender;

        public ChangeEmailCommandHandler(IClock clock,
            ITransactionManager tranManager,
            IUserRepository users,
            ITokenRepository tokens,
            IEmailSender emailSender)
        {
            _clock = clock;
            _tranManager = tranManager;
            _users = users;
            _tokens = tokens;
            _emailSender = emailSender;
        }

        public void Handle(ChangeEmailCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var user = _users.Find(command.UserId);
            if (user == null)
                throw new ApplicationException($"User {command.UserId} not found");

            var token = _tokens.Find(command.ConfirmationTokenId);
            if (token == null)
                throw new ApplicationException($"Token {command.ConfirmationTokenId} not found");

            if (token.Value != command.ConfirmationToken)
                throw new ApplicationException("Token mismatch");

            if (_clock.CurrentDate > token.NotAfter)
            {
                _tokens.Delete(token.Id);
                throw new ApplicationException("Token expired");
            }

            user.Email = command.NewEmail;

            using var t = _tranManager.BeginTransaction();
            _users.Save(user);
            _tokens.Delete(token.Id);
            t.Commit();

            _emailSender.SendEmail(user.Email, "Email changed", $"Your email has been changed to {user.Email}");
        }
    }
}