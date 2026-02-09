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
            try
            {
                // Return a defensive copy to prevent external modifications
                var eventsCopy = events.Select(e => e).ToList();
                return Task.FromResult(eventsCopy);
            }
            catch (Exception ex)
            {
                // Log error in production
                Console.WriteLine($"Error retrieving events: {ex.Message}");
                return Task.FromResult(new List<Event>());
            }
        }

        public Task<Event?> GetEventByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Task.FromResult<Event?>(null);
                }

                var evt = events.FirstOrDefault(e => e != null && e.Id == id);
                return Task.FromResult(evt);
            }
            catch (Exception ex)
            {
                // Log error in production
                Console.WriteLine($"Error retrieving event {id}: {ex.Message}");
                return Task.FromResult<Event?>(null);
            }
        }

        public Task<bool> RegisterForEventAsync(int eventId, string attendeeName, string attendeeEmail)
        {
            try
            {
                // Validate inputs
                if (eventId <= 0)
                {
                    throw new ArgumentException("Invalid event ID", nameof(eventId));
                }

                if (string.IsNullOrWhiteSpace(attendeeName))
                {
                    throw new ArgumentException("Attendee name is required", nameof(attendeeName));
                }

                if (string.IsNullOrWhiteSpace(attendeeEmail))
                {
                    throw new ArgumentException("Attendee email is required", nameof(attendeeEmail));
                }

                // Validate email format (basic)
                if (!attendeeEmail.Contains("@") || !attendeeEmail.Contains("."))
                {
                    throw new ArgumentException("Invalid email format", nameof(attendeeEmail));
                }

                var evt = events.FirstOrDefault(e => e != null && e.Id == eventId);
                
                if (evt == null)
                {
                    throw new InvalidOperationException($"Event with ID {eventId} not found");
                }

                if (!evt.IsValid())
                {
                    throw new InvalidOperationException("Event data is invalid");
                }

                if (evt.IsPastEvent())
                {
                    throw new InvalidOperationException("Cannot register for past events");
                }

                if (evt.RegisteredAttendees >= evt.Capacity)
                {
                    throw new InvalidOperationException("Event is at full capacity");
                }

                evt.RegisteredAttendees++;
                return Task.FromResult(true);
            }
            catch (ArgumentException)
            {
                // Re-throw validation exceptions
                throw;
            }
            catch (InvalidOperationException)
            {
                // Re-throw business logic exceptions
                throw;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                Console.WriteLine($"Unexpected error during registration: {ex.Message}");
                return Task.FromResult(false);
            }
        }

        public bool HasAvailableSpots(Event evt)
        {
            if (evt == null || !evt.IsValid())
            {
                return false;
            }

            return evt.RegisteredAttendees < evt.Capacity && !evt.IsPastEvent();
        }

        public int GetAvailableSpots(Event evt)
        {
            if (evt == null || !evt.IsValid())
            {
                return 0;
            }

            return Math.Max(0, evt.Capacity - evt.RegisteredAttendees);
        }
    }
}
