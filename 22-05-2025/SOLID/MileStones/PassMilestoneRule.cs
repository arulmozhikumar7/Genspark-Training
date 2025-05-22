using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;

namespace FootballStatsApp.Milestones
{
    public class PassMilestoneRule : IMilestoneRule
    {
        private readonly INotifier _notifier;

        public PassMilestoneRule(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void Evaluate(MatchStat stat)
        {
            if (stat.Passes >= 100)
            {
                _notifier.Notify($"Player {stat.PlayerId} made 100+ passes in match {stat.MatchId}!");
            }
        }
    }
}
