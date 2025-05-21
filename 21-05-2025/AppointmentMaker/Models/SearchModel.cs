namespace AppointmentMaker.Models
{
    public class SearchModel
    {
        public string? PatientName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public Range<int>? Age { get; set; }
    }

    public class Range<T> where T : struct, IComparable<T>
    {
        public T? MinVal { get; set; }
        public T? MaxVal { get; set; }

        public bool IsWithinRange(T value)
        {
            if (MinVal.HasValue && value.CompareTo(MinVal.Value) < 0)
                return false;
            if (MaxVal.HasValue && value.CompareTo(MaxVal.Value) > 0)
                return false;
            return true;
        }
    }
}
