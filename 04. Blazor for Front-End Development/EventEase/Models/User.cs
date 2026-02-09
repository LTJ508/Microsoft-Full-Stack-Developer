using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastActiveAt { get; set; } = DateTime.Now;
        
        // User's registered events
        public List<int> RegisteredEventIds { get; set; } = new();
    }
    
    public class UserSession
    {
        public User? CurrentUser { get; set; }
        public bool IsAuthenticated => CurrentUser != null;
        public DateTime SessionStartTime { get; set; } = DateTime.Now;
        public int EventsViewed { get; set; } = 0;
        public int EventsRegistered { get; set; } = 0;
    }
    
    public class EventRegistration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string? SpecialRequests { get; set; }
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Confirmed;
        public bool CheckedIn { get; set; } = false;
        public DateTime? CheckInTime { get; set; }
    }
    
    public enum RegistrationStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Attended,
        NoShow
    }
}
