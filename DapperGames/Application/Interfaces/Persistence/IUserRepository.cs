using System.Collections.Generic;
using DapperGames.Domain;

namespace DapperGames.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        User Find(int id);
        void Save(User user);
        IEnumerable<User> GetAll(string emailFilter, string nameFilter, int limit);
    }
}