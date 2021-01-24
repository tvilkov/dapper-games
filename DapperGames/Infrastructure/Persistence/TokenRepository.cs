using System;
using Dapper;
using DapperGames.Application.Interfaces.Persistence;
using DapperGames.Domain;

namespace DapperGames.Infrastructure.Persistence
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IDbSession _session;

        public TokenRepository(IDbSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Token Find(int id)
        {
            var token = _session.Connection.QueryFirst<Token>("select * from Tokens where Id=@id", new {id},
                _session.CurrentTransaction);
            return token;
        }

        public void Delete(int id)
        {
            _session.Connection.Execute("delete from Tokens where Id=@id", new {id}, _session.CurrentTransaction);
        }
    }
}