using SQLite;

namespace LocationHeatMapApp.Models
{
    public class UserLocation
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimeStamp { get; set; }

        public UserLocation() 
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
        }
    }
}
