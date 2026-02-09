using EventEase.Models;

namespace EventEase.Services
{
    public class EventService
    {
        private List<Event> events = new()
        {
            new Event
            {
                Id = 1,
                Name = "Tech Conference 2026",
                Date = new DateTime(2026, 3, 15, 9, 0, 0),
                Location = "Seattle Convention Center, WA",
                Description = "Join us for the premier technology conference featuring keynotes from industry leaders, hands-on workshops, and networking opportunities.",
                Capacity = 500,
                RegisteredAttendees = 342
            },
            new Event
            {
                Id = 2,
                Name = "Summer Music Festival",
                Date = new DateTime(2026, 6, 20, 14, 0, 0),
                Location = "Central Park, New York, NY",
                Description = "Experience an unforgettable day of live music performances from top artists across multiple genres in a beautiful outdoor setting.",
                Capacity = 2000,
                RegisteredAttendees = 1756
            },
            new Event
            {
                Id = 3,
                Name = "Corporate Training Workshop",
                Date = new DateTime(2026, 4, 10, 8, 30, 0),
                Location = "Hilton Hotel, Chicago, IL",
                Description = "Professional development workshop focusing on leadership skills, team collaboration, and modern workplace strategies.",
                Capacity = 100,
                RegisteredAttendees = 87
            },
            new Event
            {
                Id = 4,
                Name = "Art Exhibition Gala",
                Date = new DateTime(2026, 5, 5, 18, 0, 0),
                Location = "Museum of Modern Art, Los Angeles, CA",
                Description = "Exclusive evening showcase of contemporary art featuring emerging artists, with wine tasting and networking reception.",
                Capacity = 150,
                RegisteredAttendees = 98
            },
            new Event
            {
                Id = 5,
                Name = "Charity Run for Education",
                Date = new DateTime(2026, 7, 12, 7, 0, 0),
                Location = "Waterfront Trail, Boston, MA",
                Description = "5K and 10K charity run supporting local education initiatives. All fitness levels welcome with prizes for top finishers.",
                Capacity = 1000,
                RegisteredAttendees = 623
            },
            new Event
            {
                Id = 6,
                Name = "Food & Wine Tasting",
                Date = new DateTime(2026, 8, 18, 17, 0, 0),
                Location = "Napa Valley Resort, CA",
                Description = "Gourmet culinary experience featuring award-winning chefs and premium wine selections from local vineyards.",
                Capacity = 200,
                RegisteredAttendees = 145
            }
        };

        public Task<List<Event>> GetEventsAsync()
        {
            return Task.FromResult(events);
        }

        public Task<Event?> GetEventByIdAsync(int id)
        {
            var evt = events.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(evt);
        }

        public Task<bool> RegisterForEventAsync(int eventId, string attendeeName, string attendeeEmail)
        {
            var evt = events.FirstOrDefault(e => e.Id == eventId);
            if (evt != null && evt.RegisteredAttendees < evt.Capacity)
            {
                evt.RegisteredAttendees++;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public bool HasAvailableSpots(Event evt)
        {
            return evt.RegisteredAttendees < evt.Capacity;
        }

        public int GetAvailableSpots(Event evt)
        {
            return evt.Capacity - evt.RegisteredAttendees;
        }
    }
}
