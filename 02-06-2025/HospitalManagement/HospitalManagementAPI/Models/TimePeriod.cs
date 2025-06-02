using System;

namespace HospitalManagementAPI.Models
{
    public class TimePeriod
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public TimePeriod(TimeSpan start, TimeSpan end)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time.");

            Start = start;
            End = end;
        }
    }
}
