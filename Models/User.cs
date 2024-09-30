using MongoDB.Bson;

namespace WorkingHoursCounterSystemCore.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string AuthToken { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public UserSettings UserSettings { get; set; } = new UserSettings();

        public DateTime CreatedAtUtc { get; set; }
    }
}