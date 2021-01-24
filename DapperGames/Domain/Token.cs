using System;

namespace DapperGames.Domain
{
    public class Token
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTimeOffset NotAfter { get; set; }
    }
}