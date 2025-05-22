using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;
using System;
using System.Linq;

namespace FootballStatsApp.Services
{
    public class TeamManager
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IStatRepository _statRepo;
        private readonly MilestoneChecker _milestoneChecker;

        public TeamManager(IPlayerRepository playerRepo, IStatRepository statRepo, MilestoneChecker milestoneChecker)
        {
            _playerRepo = playerRepo;
            _statRepo = statRepo;
            _milestoneChecker = milestoneChecker;
        }

        public void AddPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            _playerRepo.AddPlayer(player);
            Console.WriteLine($"Added player: {player.Name}");
        }

        public void AddMatchStat(MatchStat stat)
        {
            if (stat == null) throw new ArgumentNullException(nameof(stat));
            var player = _playerRepo.GetPlayer(stat.PlayerId);
            if (player == null)
            {
                Console.WriteLine($"Player with Id {stat.PlayerId} not found.");
                return;
            }

            _statRepo.AddMatchStat(stat);
            _milestoneChecker.Check(stat);
        }

        public int GetMatchesPlayed(int playerId)
        {
            return _statRepo.GetMatchesPlayed(playerId);
        }

        public void ShowPlayerStats(int playerId)
        {
            var player = _playerRepo.GetPlayer(playerId);
            if (player == null)
            {
                Console.WriteLine($"Player with Id {playerId} not found.");
                return;
            }

            var stats = _statRepo.GetStatsByPlayerId(playerId);
            var matchesPlayed = _statRepo.GetMatchesPlayed(playerId);

            Console.WriteLine(player);
            Console.WriteLine($"Matches Played: {matchesPlayed}");
            foreach (var stat in stats)
            {
                Console.WriteLine(stat);
            }

            var totalGoals = stats.Sum(s => s.Goals);
            var totalAssists = stats.Sum(s => s.Assists);
            var totalPasses = stats.Sum(s => s.Passes);

            Console.WriteLine($"Total Goals: {totalGoals}, Total Assists: {totalAssists}, Total Passes: {totalPasses}");
        }
    }
}
