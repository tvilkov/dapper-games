using System;
using DapperGames.Application.Interfaces.Common;

namespace DapperGames.Infrastructure.Common
{
    public class Clock : IClock
    {
        public DateTimeOffset CurrentDate => DateTimeOffset.UtcNow;
    }
}