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
  
## Installation & Setup
### Prerequisites
- Visual Studio 2026(with .NET MAUI workload)
- Android Emulator installed
- Google Maps API Key (Did not commit to this repo as it public, will paste in word doc)

### Steps
1. Clone the repository:
   ```
   git clone https://github.com/Kejmerkew/LocationHeatMapApp
   ```
2. Open solution in Visual Studio.
3. Add Google Maps API key in: `AndroidManifest.xml`
4. Ensure the following NuGet packages are installed: `sqlite-net-pcl` | `Microsoft.Maui.Controls.Maps`
5. Build and run using Android Emulator.

## How Location Tracking Works
1. User presses Start Tracking
2. Runtime location permission is requested
3. A background asynchronous loop:
   - Retrieves device GPS coordinates
   - Saves coordinates to SQLite db
   - Re-renders heat map overlays
4. Tracking repeats every 2.5 seconds
5. Pressing Stop Tracking cancels the loop via CancellationToken

## MSCS-533-A01
This project was developed as part of the final project for the *Software Engineering and Multiplatform App Development* class at UCumberlands
*Ronit Pawar*




