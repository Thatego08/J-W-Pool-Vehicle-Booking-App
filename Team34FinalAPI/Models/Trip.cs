using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team34FinalAPI.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }
        public string Name { get; set; } // Replace VehicleId with Name
        public string Location { get; set; }
        public decimal FuelAmount { get; set; }
        public string Comment { get; set; }
        public DateTime TravelStart { get; set; }
        public DateTime TravelEnd { get; set; }
        public string RegistrationNumber { get; set; }

        [JsonIgnore]
        public ICollection<TripMedia> TripMedia { get; set; }
        public string UserName { get; set; }
        public ICollection<RefuelVehicle> RefuelVehicles { get; set; }

        public bool HasAccidents { get; internal set; }
    }
}
