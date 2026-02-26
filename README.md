# LocationHeatMapApp

## Overview
The Location Heat Map App is a cross-platform mobile application built using C# and .NET MAUI.

This project demonstrates:
- Native device GPS integration
- SQLite local data persistence
- Map rendering using .NET MAUI Maps
- Asynchronous programming patterns
- Mobile permission handling
- Real-time data visualization

### Technologies Used: C#, .NET MAUI, Microsoft.Maui.Controls.Maps, SQLite-net-pcl, Android Emulator, Google Maps API

## Architecture Overview
The application follows a clean separation of concerns:
1. UI Layer: `MainPage.xaml`
   - Displays map and user controls
   - Handles interaction events

2. Business Logic Layer: `MainPage.xaml.cs`
   - Manages GPS retrieval
   - Controls tracking loop
   - Generates heat map visualization

3. Data Layer:
   - UserLocation model
   - LocationDbService for SQLite operations
   - Handles persistent storage and retrieval
