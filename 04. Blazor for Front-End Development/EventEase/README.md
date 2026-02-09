# EventEase - Event Management Application

## 📚 Course Information

**Course:** Microsoft Full-Stack Developer Professional Certificate  
**Platform:** Coursera  
**Module:** Blazor for Front-End Development  
**Activities:** 3-part progressive project (Foundation → Debugging → Advanced Features)

## 🎯 Project Overview

EventEase is a corporate and social event management web application built with Blazor WebAssembly. Users can browse events, view details, register for events, and track their attendance history.

## ✨ Key Features

### Event Discovery & Management

- **Browse Events**: Paginated list with search and sorting (Date, Name, Availability)
- **Event Details**: Comprehensive information with capacity tracking and breadcrumb navigation
- **Smart Filtering**: Real-time search across event names and locations

### Registration System

- **Validated Forms**: Email, phone, and required field validation
- **Capacity Management**: Real-time availability checking and registration limits
- **Confirmation System**: Success messages with event details

### User Dashboard

- **My Events Page**: Personal dashboard with registration history
- **Session Tracking**: Persistent user data across browser sessions (localStorage)
- **Event Tabs**: Separate views for upcoming and past events

### Attendance Tracking

- **Check-in System**: Mark attendance at events
- **Status Management**: Track registrations (Confirmed, Attended, Cancelled, No-Show)
- **Statistics**: View attendance rates and participation metrics

## 🛠️ Technology Stack

- **.NET 10.0** - Latest framework version
- **Blazor WebAssembly** - Client-side SPA framework
- **Bootstrap 5** - Responsive UI design
- **C# 13** - Modern language features
- **Dependency Injection** - Service architecture

## 📁 Project Structure

```
EventEase/
├── Components/          # Reusable UI components (EventCard)
├── Models/             # Data models (Event, User, Registration)
├── Services/           # Business logic layer (EventService, UserSession, Attendance)
├── Pages/              # Routable pages (Home, Events, Details, Registration, MyEvents)
└── Layout/             # Application layout and navigation
```

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK or higher
- Visual Studio 2022 or VS Code

### Run the Application

```bash
cd EventEase
dotnet restore
dotnet build
dotnet run
```
Navigate to `https://localhost:5001` in your browser.

## 🎓 Learning Objectives

### Activity 1: Foundation

- Component-based architecture with EventCard
- Data binding (one-way and two-way)
- Routing with parameters
- Dependency injection patterns

### Activity 2: Debugging & Optimization

- Input validation with DataAnnotations
- Null safety and edge case handling
- Performance optimization (pagination, rendering)
- Error handling and user feedback

### Activity 3: Advanced Features

- State management with localStorage
- User session tracking
- Complex form validation
- Production-ready code practices

## 📊 Mock Data

Includes 6 diverse events:

- Tech Innovation Summit 2026
- Summer Music Festival
- Corporate Leadership Training
- Annual Art Gala
- Charity Marathon Run
- Wine Tasting Evening

## 🏆 Key Achievements

- ✅ Zero build errors/warnings
- ✅ 30+ edge cases handled
- ✅ Responsive design (mobile-first)
- ✅ Production-ready validation
- ✅ Persistent user sessions
- ✅ Comprehensive error handling
