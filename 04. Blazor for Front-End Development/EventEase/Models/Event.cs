using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Event name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Event name must be between 3 and 200 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Location is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Location must be between 5 and 300 characters")]
        public string Location { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
        public string Description { get; set; } = string.Empty;
        
        [Range(1, 100000, ErrorMessage = "Capacity must be between 1 and 100,000")]
        public int Capacity { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Registered attendees cannot be negative")]
        public int RegisteredAttendees { get; set; }
        
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = "/images/event-placeholder.jpg";
        
        // Validation method
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Location) &&
                   !string.IsNullOrWhiteSpace(Description) &&
                   Capacity > 0 &&
                   RegisteredAttendees >= 0 &&
                   RegisteredAttendees <= Capacity &&
                   Date > DateTime.MinValue;
        }
        
        public bool IsPastEvent()
        {
            return Date < DateTime.Now;
        }
    }
}
