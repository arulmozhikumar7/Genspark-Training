using FootballStatsApp.Interfaces;
using System;

namespace FootballStatsApp.Notifications
{
    public class ConsoleNotifier : INotifier
    {
        public void Notify(string message)
        {
            Console.WriteLine($"[NOTIFICATION]: {message}");
        }
    }
}
