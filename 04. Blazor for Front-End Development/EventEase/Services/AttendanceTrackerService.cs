using EventEase.Models;

namespace EventEase.Services
{
    public class AttendanceTrackerService
    {
        private List<EventRegistration> _registrations = new();
        private int _nextId = 1;
        
        public event Action? OnAttendanceChanged;
        
        public Task<List<EventRegistration>> GetAllRegistrationsAsync()
        {
            return Task.FromResult(_registrations.OrderByDescending(r => r.RegistrationDate).ToList());
        }
        
        public Task<List<EventRegistration>> GetRegistrationsByEventIdAsync(int eventId)
        {
            var registrations = _registrations.Where(r => r.EventId == eventId).ToList();
            return Task.FromResult(registrations);
        }
        
        public Task<List<EventRegistration>> GetRegistrationsByUserIdAsync(string userId)
        {
            var registrations = _registrations.Where(r => r.UserId == userId).ToList();
            return Task.FromResult(registrations);
        }
        
        public Task<List<EventRegistration>> GetRegistrationsByUserEmailAsync(string email)
        {
            var registrations = _registrations
                .Where(r => r.UserEmail.Equals(email, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(registrations);
        }
        
        public Task<EventRegistration?> GetRegistrationAsync(int eventId, string userId)
        {
            var registration = _registrations.FirstOrDefault(r => r.EventId == eventId && r.UserId == userId);
            return Task.FromResult(registration);
        }
        
        public async Task<EventRegistration> CreateRegistrationAsync(
            int eventId, 
            string userId, 
            string userName, 
            string userEmail,
            string? specialRequests = null)
        {
            // Check if already registered
            var existing = await GetRegistrationAsync(eventId, userId);
            if (existing != null)
            {
                return existing;
            }
            
            var registration = new EventRegistration
            {
                Id = _nextId++,
                EventId = eventId,
                UserId = userId,
                UserName = userName,
                UserEmail = userEmail,
                RegistrationDate = DateTime.Now,
                SpecialRequests = specialRequests,
                Status = RegistrationStatus.Confirmed
            };
            
            _registrations.Add(registration);
            NotifyAttendanceChanged();
            
            return registration;
        }
        
        public async Task<bool> CheckInAsync(int eventId, string userId)
        {
            var registration = await GetRegistrationAsync(eventId, userId);
            if (registration != null && !registration.CheckedIn)
            {
                registration.CheckedIn = true;
                registration.CheckInTime = DateTime.Now;
                registration.Status = RegistrationStatus.Attended;
                NotifyAttendanceChanged();
                return true;
            }
            return false;
        }
        
        public async Task<bool> CancelRegistrationAsync(int eventId, string userId)
        {
            var registration = await GetRegistrationAsync(eventId, userId);
            if (registration != null && registration.Status == RegistrationStatus.Confirmed)
            {
                registration.Status = RegistrationStatus.Cancelled;
                NotifyAttendanceChanged();
                return true;
            }
            return false;
        }
        
        public async Task<bool> MarkNoShowAsync(int eventId, string userId)
        {
            var registration = await GetRegistrationAsync(eventId, userId);
            if (registration != null && !registration.CheckedIn)
            {
                registration.Status = RegistrationStatus.NoShow;
                NotifyAttendanceChanged();
                return true;
            }
            return false;
        }
        
        public Task<int> GetEventAttendanceCountAsync(int eventId)
        {
            var count = _registrations.Count(r => r.EventId == eventId && r.CheckedIn);
            return Task.FromResult(count);
        }
        
        public Task<int> GetEventRegistrationCountAsync(int eventId)
        {
            var count = _registrations.Count(r => 
                r.EventId == eventId && 
                r.Status != RegistrationStatus.Cancelled);
            return Task.FromResult(count);
        }
        
        public Task<double> GetEventAttendanceRateAsync(int eventId)
        {
            var registrations = _registrations.Where(r => 
                r.EventId == eventId && 
                r.Status != RegistrationStatus.Cancelled).ToList();
            
            if (!registrations.Any())
                return Task.FromResult(0.0);
            
            var attended = registrations.Count(r => r.CheckedIn);
            var rate = (double)attended / registrations.Count * 100;
            
            return Task.FromResult(Math.Round(rate, 1));
        }
        
        public Task<Dictionary<RegistrationStatus, int>> GetEventRegistrationStatsAsync(int eventId)
        {
            var stats = _registrations
                .Where(r => r.EventId == eventId)
                .GroupBy(r => r.Status)
                .ToDictionary(g => g.Key, g => g.Count());
            
            return Task.FromResult(stats);
        }
        
        private void NotifyAttendanceChanged()
        {
            OnAttendanceChanged?.Invoke();
        }
        
        // Utility method to get user's upcoming events
        public async Task<List<EventRegistration>> GetUserUpcomingRegistrationsAsync(string userId)
        {
            var registrations = await GetRegistrationsByUserIdAsync(userId);
            return registrations
                .Where(r => r.Status != RegistrationStatus.Cancelled)
                .OrderBy(r => r.RegistrationDate)
                .ToList();
        }
        
        // Get attendance summary for analytics
        public Task<AttendanceSummary> GetAttendanceSummaryAsync()
        {
            var summary = new AttendanceSummary
            {
                TotalRegistrations = _registrations.Count,
                ConfirmedRegistrations = _registrations.Count(r => r.Status == RegistrationStatus.Confirmed),
                CancelledRegistrations = _registrations.Count(r => r.Status == RegistrationStatus.Cancelled),
                AttendedCount = _registrations.Count(r => r.Status == RegistrationStatus.Attended),
                NoShowCount = _registrations.Count(r => r.Status == RegistrationStatus.NoShow),
                CheckedInCount = _registrations.Count(r => r.CheckedIn)
            };
            
            if (summary.TotalRegistrations > 0)
            {
                summary.AttendanceRate = Math.Round((double)summary.AttendedCount / summary.TotalRegistrations * 100, 1);
            }
            
            return Task.FromResult(summary);
        }
    }
    
    public class AttendanceSummary
    {
        public int TotalRegistrations { get; set; }
        public int ConfirmedRegistrations { get; set; }
        public int CancelledRegistrations { get; set; }
        public int AttendedCount { get; set; }
        public int NoShowCount { get; set; }
        public int CheckedInCount { get; set; }
        public double AttendanceRate { get; set; }
    }
}
