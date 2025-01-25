namespace ModelsLibrary.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public required string Type { get; set; } // Type of notification (e.g., "Follow", "Like")
        public required string ReceiverUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
