using EventEase.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace EventEase.Services
{
    public class UserSessionService
    {
        private readonly IJSRuntime _jsRuntime;
        private UserSession _session = new();
        private const string SessionStorageKey = "eventease_user_session";
        
        public event Action? OnSessionChanged;
        
        public UserSessionService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        
        public UserSession CurrentSession => _session;
        
        public User? CurrentUser => _session.CurrentUser;
        
        public bool IsAuthenticated => _session.IsAuthenticated;
        
        public async Task InitializeAsync()
        {
            try
            {
                var sessionJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", SessionStorageKey);
                
                if (!string.IsNullOrEmpty(sessionJson))
                {
                    var session = JsonSerializer.Deserialize<UserSession>(sessionJson);
                    if (session != null)
                    {
                        _session = session;
                        NotifySessionChanged();
                    }
                }
            }
            catch (Exception)
            {
                // In production, use ILogger for proper error logging
            }
        }
        
        public async Task<User> CreateOrUpdateUserAsync(string fullName, string email, string? phone = null, string? company = null)
        {
            if (_session.CurrentUser != null && _session.CurrentUser.Email == email)
            {
                // Update existing user
                _session.CurrentUser.FullName = fullName;
                _session.CurrentUser.Phone = phone;
                _session.CurrentUser.Company = company;
                _session.CurrentUser.LastActiveAt = DateTime.Now;
            }
            else
            {
                // Create new user
                _session.CurrentUser = new User
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Company = company,
                    CreatedAt = DateTime.Now,
                    LastActiveAt = DateTime.Now
                };
                _session.SessionStartTime = DateTime.Now;
            }
            
            await SaveSessionAsync();
            NotifySessionChanged();
            
            return _session.CurrentUser;
        }
        
        public async Task AddEventRegistrationAsync(int eventId)
        {
            if (_session.CurrentUser != null && !_session.CurrentUser.RegisteredEventIds.Contains(eventId))
            {
                _session.CurrentUser.RegisteredEventIds.Add(eventId);
                _session.EventsRegistered++;
                _session.CurrentUser.LastActiveAt = DateTime.Now;
                
                await SaveSessionAsync();
                NotifySessionChanged();
            }
        }
        
        public async Task TrackEventViewAsync()
        {
            _session.EventsViewed++;
            await SaveSessionAsync();
        }
        
        public List<int> GetUserRegisteredEvents()
        {
            return _session.CurrentUser?.RegisteredEventIds ?? new List<int>();
        }
        
        public bool IsUserRegisteredForEvent(int eventId)
        {
            return _session.CurrentUser?.RegisteredEventIds.Contains(eventId) ?? false;
        }
        
        public async Task LogoutAsync()
        {
            _session = new UserSession();
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", SessionStorageKey);
            NotifySessionChanged();
        }
        
        public async Task ClearSessionAsync()
        {
            await LogoutAsync();
        }
        
        private async Task SaveSessionAsync()
        {
            try
            {
                var sessionJson = JsonSerializer.Serialize(_session);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", SessionStorageKey, sessionJson);
            }
            catch (Exception)
            {
                // In production, use ILogger for proper error logging
            }
        }
        
        private void NotifySessionChanged()
        {
            OnSessionChanged?.Invoke();
        }
        
        public TimeSpan GetSessionDuration()
        {
            return DateTime.Now - _session.SessionStartTime;
        }
        
        public string GetSessionSummary()
        {
            var duration = GetSessionDuration();
            return $"Session Duration: {duration.Hours}h {duration.Minutes}m | Events Viewed: {_session.EventsViewed} | Registered: {_session.EventsRegistered}";
        }
    }
}
