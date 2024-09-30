using MongoDB.Bson;

namespace WorkingHoursCounterSystemCore.Models
{
    public class Shift
    {
        public ObjectId Id { get; set; }

        public ObjectId AuthorId { get; set; }

        public DateTime StartTimeUtc { get; set; }

        public DateTime EndTimeUtc { get; set; }

        public double SalaryPerHour { get; set; }

        public TimeSpan WorkedTimeInterval { get; set; }
    }
}