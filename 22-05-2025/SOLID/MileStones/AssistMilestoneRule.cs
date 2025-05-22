using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;

namespace FootballStatsApp.Milestones
{
    public class AssistMilestoneRule : IMilestoneRule
    {
        private readonly INotifier _notifier;

        public AssistMilestoneRule(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void Evaluate(MatchStat stat)
        {
            if (stat.Assists >= 10)
            {
                _notifier.Notify($"Player {stat.PlayerId} made 10+ assists in match {stat.MatchId}!");
            }
        }
    }
}
