using DapperGames.Domain;

namespace DapperGames.Application.Interfaces.Persistence
{
    public interface ITokenRepository
    {
        Token Find(int id);
        void Delete(int id);
    }
}