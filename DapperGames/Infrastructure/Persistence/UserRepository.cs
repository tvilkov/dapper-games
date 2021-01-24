using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using DapperGames.Application.Interfaces.Persistence;
using DapperGames.Domain;

namespace DapperGames.Infrastructure.Persistence
{
    internal class UserRepository : IUserRepository
    {
        private readonly IDbSession _session;

        public UserRepository(IDbSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public User Find(int id)
        {
            var user = _session.Connection.QueryFirst<User>("select * from Users where Id=@id", new {id},
                _session.CurrentTransaction);
            return user;
        }

        public void Save(User user)
        {
            if (user.Id == 0)
            {
                _session.Connection.Execute("update Users set Email=@Email, Name=@Name where Id=@Id", user,
                    _session.CurrentTransaction);
            }
            else
            {
                var id = _session.Connection.ExecuteScalar<int>(
                    "insert into Users(Email, Name)" +
                    " output INSERTED.Id" +
                    " values (@Email, @Name)", user,
                    _session.CurrentTransaction);
                user.Id = id;
            }
        }

        public IEnumerable<User> GetAll(string emailFilter, string nameFilter, int limit)
        {
            var users = _session.Connection.Query<User>(
                "select top @limit * from Users" +
                " where Email like '%@email' or Name like '%@name'",
                new {email = emailFilter, name = nameFilter, limit});
            return users;
        }
    }
}