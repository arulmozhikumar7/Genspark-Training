using FootballStatsApp.Models;
using FootballStatsApp.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FootballStatsApp.Repositories
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private readonly List<Player> _players = new();

        public void AddPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (_players.Any(p => p.Id == player.Id)) 
                throw new ArgumentException($"Player with Id {player.Id} already exists.");
            _players.Add(player);
        }

        public Player? GetPlayer(int id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Player> GetAllPlayers() => _players;
    }
}
