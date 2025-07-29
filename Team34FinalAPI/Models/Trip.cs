using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team34FinalAPI.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }
        public int BookingID { get; set; }  // Foreign key property
        [JsonIgnore]
        public Booking Booking { get; set; }
        public string Name { get; set; } // Replace VehicleId with Name
        public string Location { get; set; }
     
        public string Comment { get; set; }
        public DateTime TravelStart { get; set; }
        public DateTime TravelEnd { get; set; }

        // Foreign key for PreChecklist
        public int? PreChecklistId { get; set; }
        public PreChecklist PreChecklist { get; set; } // Navigation property
      
      
        public string UserName { get; set; }
        public ICollection<RefuelVehicle> RefuelVehicles { get; set; }
        public ICollection<PostCheck> PostChecks { get; set; } = new List<PostCheck>(); // Navigation property for PostChecks


    }
}
