using System.Data;

namespace DapperGames.Application.Interfaces.Persistence
{
    public interface ITransactionManager
    {
        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}