using FootballStatsApp.Models;
using System.Collections.Generic;

namespace FootballStatsApp.Interfaces
{
    public interface IStatRepository
    {
        void AddMatchStat(MatchStat stat);
        IEnumerable<MatchStat> GetStatsByPlayerId(int playerId);
        int GetMatchesPlayed(int playerId);
    }
}
