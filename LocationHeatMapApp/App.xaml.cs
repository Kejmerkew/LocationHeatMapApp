using LocationHeatMapApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LocationHeatMapApp
{
    public partial class App : Application
    {
        public static LocationDbService locationDbService { get; private set; }

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "locations.db3");

            locationDbService = new LocationDbService(dbPath);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}