using FootballStatsApp.Models;

namespace FootballStatsApp.Interfaces
{
    public interface IMilestoneRule
    {
        void Evaluate(MatchStat stat);
    }
}
