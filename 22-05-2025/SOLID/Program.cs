using FootballStatsApp.Models;
using FootballStatsApp.Repositories;
using FootballStatsApp.Services;
using FootballStatsApp.Notifications;
using FootballStatsApp.Milestones;
using FootballStatsApp.Interfaces;
class Program
{
    static void Main()
    {
        var playerRepo = new InMemoryPlayerRepository();
        var statRepo = new InMemoryStatRepository();
        var notifier = new ConsoleNotifier();
        var milestoneRules = new List<IMilestoneRule>
        {
            new HattrickMilestoneRule(notifier),
            new AssistMilestoneRule(notifier),
            new PassMilestoneRule(notifier)
        };
        var milestoneChecker = new MilestoneChecker(milestoneRules);

        var manager = new TeamManager(playerRepo, statRepo, milestoneChecker);

        var player = new Player { Id = 1, Name = "Cristiano Ronaldo", Position = "Forward" };
        manager.AddPlayer(player);

        manager.AddMatchStat(new MatchStat { PlayerId = 1, MatchId = 101, Goals = 2, Assists = 1, Passes = 30 });
        manager.AddMatchStat(new MatchStat { PlayerId = 1, MatchId = 102, Goals = 3, Assists = 2, Passes = 40 });
        manager.AddMatchStat(new MatchStat { PlayerId = 1, MatchId = 103, Goals = 6, Assists = 8, Passes = 50 }); 

        manager.ShowPlayerStats(1);
    }
}
