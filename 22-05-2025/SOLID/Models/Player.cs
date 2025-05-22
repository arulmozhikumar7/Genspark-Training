namespace FootballStatsApp.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        public override string ToString() => $"[{Id}] {Name} - {Position}";
    }
}