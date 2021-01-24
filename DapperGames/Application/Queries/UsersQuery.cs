using System.Collections.Generic;
using DapperGames.Application.Interfaces.Cqrs;
using DapperGames.Application.Interfaces.Persistence;
using DapperGames.Domain;

namespace DapperGames.Application.Queries
{
    public class UsersQuery : IQuery<IEnumerable<User>>
    {
        public string NameFilter;
        public string EmailFilter;
        public int Limit;
    }

    public class UsersQueryHandler : IQueryHandler<UsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _users;

        public UsersQueryHandler(IUserRepository users)
        {
            _users = users;
        }

        public IEnumerable<User> Handle(UsersQuery query)
        {
            var users = _users.GetAll(query.EmailFilter, query.NameFilter, query.Limit);
            return users;
        }
    }
}