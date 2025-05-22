using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;
using System.Collections.Generic;

namespace FootballStatsApp.Services
{
    public class MilestoneChecker
    {
        private readonly IEnumerable<IMilestoneRule> _rules;

        public MilestoneChecker(IEnumerable<IMilestoneRule> rules)
        {
            _rules = rules;
        }

        public void Check(MatchStat stat)
        {
            foreach (var rule in _rules)
            {
                rule.Evaluate(stat);
            }
        }
    }
}
