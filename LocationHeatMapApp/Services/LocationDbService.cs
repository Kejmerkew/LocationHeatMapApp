using SQLite;
using LocationHeatMapApp.Models;

namespace LocationHeatMapApp.Services
{
    public  class LocationDbService
    {
        private readonly SQLiteAsyncConnection _database;

        public LocationDbService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UserLocation>().Wait();
        }

        public Task<int> SaveLocationAsync(UserLocation location)
        {
            return _database.InsertAsync(location);
        }

        public Task<List<UserLocation>> GetLocationsAsync()
        {
            return _database.Table<UserLocation>().ToListAsync();
        }

        public Task<int> ClearLocationsAsync()
        {
            return _database.DeleteAllAsync<UserLocation>();
        }
    }
}
