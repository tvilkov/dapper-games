using DapperGames.Application.Commands;
using DapperGames.Application.Interfaces.Common;
using DapperGames.Application.Interfaces.Notification;
using DapperGames.Application.Interfaces.Persistence;
using DapperGames.Application.Queries;
using DapperGames.Infrastructure;
using DapperGames.Infrastructure.Common;
using DapperGames.Infrastructure.Persistence;

namespace DapperGames
{
    class Program
    {
        static void Main(string[] args)
        {
            RunScenario1();
            RunScenario2();
        }

        public static void RunScenario1()
        {
            IDbSession db = new DbSession("localhost", "tests");
            IUserRepository users = new UserRepository(db);
            ITokenRepository tokens = new TokenRepository(db);
            IClock clock = new Clock();
            IEmailSender emailSender = new EmailSender();

            var handler = new ChangeEmailCommandHandler(clock, db, users, tokens, emailSender);
            handler.Handle(new ChangeEmailCommand
            {
                UserId = 1,
                ConfirmationTokenId = 1,
                ConfirmationToken = "123456",
                NewEmail = "new.email@google.com"
            });
        }

        public static void RunScenario2()
        {
            IDbSession db = new DbSession("localhost", "tests");
            IUserRepository users = new UserRepository(db);

            var handler = new UsersQueryHandler(users);
            handler.Handle(new UsersQuery()
            {
                EmailFilter = "user01@",
                NameFilter = "",
                Limit = 10
            });
        }
    }
}