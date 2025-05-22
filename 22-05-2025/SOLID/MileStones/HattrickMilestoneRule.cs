using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;

namespace FootballStatsApp.Milestones
{
    public class HattrickMilestoneRule : IMilestoneRule
    {
        private readonly INotifier _notifier;

        public HattrickMilestoneRule(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void Evaluate(MatchStat stat)
        {
            if (stat.Goals >= 3)
            {
                _notifier.Notify($"Player {stat.PlayerId} scored Hat-trick in match {stat.MatchId}!");
            }
        }
    }
}
