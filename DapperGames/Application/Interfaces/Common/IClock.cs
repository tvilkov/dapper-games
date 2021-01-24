using System;

namespace DapperGames.Application.Interfaces.Common
{
    public interface IClock
    {
        DateTimeOffset CurrentDate { get; }
    }
}