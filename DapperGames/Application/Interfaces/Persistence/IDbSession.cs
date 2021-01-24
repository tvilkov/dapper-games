using System;
using System.Data;

namespace DapperGames.Application.Interfaces.Persistence
{
    public interface IDbSession : ITransactionManager, IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction CurrentTransaction { get; }
    }
}