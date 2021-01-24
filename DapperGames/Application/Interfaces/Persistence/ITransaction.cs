using System;

namespace DapperGames.Application.Interfaces.Persistence
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}