using FootballStatsApp.Models;
using System.Collections.Generic;

namespace FootballStatsApp.Interfaces
{
    public interface IPlayerRepository
    {
        void AddPlayer(Player player);
        Player? GetPlayer(int id);
        IEnumerable<Player> GetAllPlayers();
    }
}
