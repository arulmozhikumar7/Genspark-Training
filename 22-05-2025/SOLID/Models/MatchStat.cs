namespace FootballStatsApp.Models
{
    public class MatchStat
    {
        public int PlayerId { get; set; }
        public int MatchId { get; set; }  
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Passes { get; set; }

        public override string ToString() =>
            $"MatchStat(PlayerId: {PlayerId}, MatchId: {MatchId}, Goals: {Goals}, Assists: {Assists}, Passes: {Passes})";
    }
}
