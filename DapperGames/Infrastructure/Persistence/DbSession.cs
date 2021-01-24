using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DapperGames.Application.Interfaces.Persistence;

namespace DapperGames.Infrastructure.Persistence
{
    public class DbSession : IDbSession
    {
        private readonly string _connString;
        private readonly string _dbName;
        private IDbConnection _conn;
        private readonly Stack<IDbTransaction> _openTransactions = new Stack<System.Data.IDbTransaction>();

        public DbSession(string connString, string dbName)
        {
            _connString = connString ?? throw new ArgumentNullException(nameof(connString));
            _dbName = dbName ?? throw new ArgumentNullException(nameof(dbName));
        }

        public IDbConnection Connection
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new SqlConnection(_connString);
                    _conn.Open();
                    _conn.ChangeDatabase(_dbName);
                }

                return _conn;
            }
        }

        public IDbTransaction CurrentTransaction => _openTransactions.Count > 0 ? _openTransactions.Peek() : null;

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var tran = Connection.BeginTransaction(isolationLevel);
            _openTransactions.Push(tran);
            return new TransactionWrapper(tran, HandleTransactionDisposal);
        }

        public void Dispose()
        {
            while (_openTransactions.TryPeek(out var tran))
            {
                tran.Rollback();
                tran.Dispose();
            }

            _conn.Dispose();
        }

        private void HandleTransactionDisposal(IDbTransaction transaction)
        {
            if (_openTransactions.Pop() != transaction)
                throw new InvalidOperationException("Invalid transaction nesting");
            transaction.Dispose();
        }

        private sealed class TransactionWrapper : ITransaction
        {
            private readonly IDbTransaction _inner;
            private readonly Action<IDbTransaction> _onDispose;

            public TransactionWrapper(IDbTransaction inner, Action<System.Data.IDbTransaction> onDispose)
            {
                _inner = inner;
                _onDispose = onDispose;
            }

            public void Commit() => _inner.Commit();

            public void Dispose() => _onDispose(_inner);
        }
    }
}