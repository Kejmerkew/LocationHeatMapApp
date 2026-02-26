using LocationHeatMapApp.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace LocationHeatMapApp
{
    public partial class MainPage : ContentPage
    {
        private bool _isTracking = false;
        private CancellationTokenSource _trackingTokenSource;

        private double StartingLat = 40.757975;
        private double StartingLong = -73.985543;

        public MainPage()
        {
            InitializeComponent();
        }

        // This method executes each time the page becomes visible (Maui Lifecycle)
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Center map on NYC initially
            var position = new Location(StartingLat, StartingLong);
            UserMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(2)));

            // Load previously stored location data and render the heat map visualization
            await LoadHeatMap();
        }

        // Handles the Start/Stop Tracking button click event
        // This method toggles GPS tracking on and off
        public async void OnStartStopClicked(object sender, EventArgs e)
        {
            // If tracking is not currently active, begin tracking
            if (!_isTracking)
            {
                // Request runtime permission to access device location
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                // If permission is denied, inform the user and stop execution
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlertAsync("Permission Denied", "Location permission is required.", "OK");
                    return;
                }

                _isTracking = true;
                StartStopButton.Text = "Stop Tracking";

                // Create a cancellation token to allow tracking loop to stop gracefully
                _trackingTokenSource = new CancellationTokenSource();
                _ = StartTrackingAsync(_trackingTokenSource.Token);
            }
            else
            {
                _isTracking = false;
                StartStopButton.Text = "Start Tracking";

                // Signal cancellation to terminate the tracking loop
                _trackingTokenSource.Cancel();
            }
        }

        // Continuously retrieves the user's GPS location at a fixed interval and stores it in the local SQLite database until cancellation is requested
        private async Task StartTrackingAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    //Medium accuracy balances precision and battery efficiency
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);

                    // Retrieve the current geographic coordinates from the device
                    var location = await Geolocation.Default.GetLocationAsync(request);

                    if (location != null)
                    {
                        // Create a new location record for database persistence
                        var userLocation = new UserLocation
                        {
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        };

                        //Persist coordinates in SQLite database
                        await App.locationDbService.SaveLocationAsync(userLocation);

                        //Refresh the heatmap visualization with updated data
                        await LoadHeatMap();

                        //Re-center the map on the latest recorded position
                        UserMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude),Distance.FromMeters(2000)));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GPS Error: {ex.Message}");
                }

                await Task.Delay(2500, token); // Save every 2.5 seconds
            }
        }

        // Retrieves all stored locations from the database and renders them as circular overlays to simulate a heat map effect
        private async Task LoadHeatMap()
        {
            //Clear existing map overlays to prevent duplication
            UserMap.MapElements.Clear();

            //Retrieve all persisted location entries
            var locations = await App.locationDbService.GetLocationsAsync();

            if (!locations.Any())  //Exit if no data exists
            {
                return;
            }

            // Group nearby coordinates by rounding to 3 decimal places
            // This clusters close points to simulate heat intensity zones
            var grouped = locations.GroupBy(l => new {
                Lat = Math.Round(l.Latitude, 3),
                Lng = Math.Round(l.Longitude, 3)
            });

            // Create a circular overlay to represent heat density
            foreach (var group in grouped)
            {
                //Determine heat intensity based on number of records in cluster
                int intensity = group.Count();

                var circle = new Circle
                {
                    Center = new Location(group.Key.Lat, group.Key.Lng),
                    Radius = new Distance(120),
                    StrokeColor = Colors.Transparent,
                    FillColor = Colors.Red.WithAlpha(Math.Min(0.5f * intensity, 1f)),
                };

                // Add circle overlay to map
                UserMap.MapElements.Add(circle);
            }
        }

        public async void OnClearClicked(object sender, EventArgs e)
        {
            await App.locationDbService.ClearLocationsAsync();
            UserMap.MapElements.Clear();
        }
    }
}
