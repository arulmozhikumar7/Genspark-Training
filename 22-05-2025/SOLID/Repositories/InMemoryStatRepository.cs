using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FootballStatsApp.Repositories
{
    public class InMemoryStatRepository : IStatRepository
    {
        private readonly List<MatchStat> _stats = new();

        public void AddMatchStat(MatchStat stat)
        {
            if (stat == null) throw new ArgumentNullException(nameof(stat));
            var exists = _stats.Any(s => s.PlayerId == stat.PlayerId && s.MatchId == stat.MatchId);
            if (exists) throw new ArgumentException($"Stats for PlayerId {stat.PlayerId} in MatchId {stat.MatchId} already exist.");

            _stats.Add(stat);
        }

        public IEnumerable<MatchStat> GetStatsByPlayerId(int playerId)
        {
            return _stats.Where(s => s.PlayerId == playerId);
        }

        public int GetMatchesPlayed(int playerId)
        {
            return _stats.Count(s => s.PlayerId == playerId);
        }
    }
}
