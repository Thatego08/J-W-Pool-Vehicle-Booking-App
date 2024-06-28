using System.ComponentModel.DataAnnotations;

namespace Team34FinalAPI.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }
        public int VehicleId { get; set; } // Foreign key
        public Vehicle Vehicle { get; set; } // Navigation property
        public string Location { get; set; }
        public decimal FuelAmount { get; set; }
        public string Comment { get; set; }
        public DateTime TravelStart { get; set; }
        public DateTime TravelEnd { get; set; }
        public string RegistrationNumber { get; set; }

        public ICollection<TripMedia> TripMedia { get; set; }
    }
}
