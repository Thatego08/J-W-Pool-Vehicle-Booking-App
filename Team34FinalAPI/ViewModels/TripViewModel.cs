namespace Team34FinalAPI.ViewModels
{
    public class TripViewModel
    {
        public int TripId { get; set; } // Required for identifying the trip to update
        public int VehicleId { get; set; }
        public string Location { get; set; }
        public decimal FuelAmount { get; set; }
        public string Comment { get; set; }
        public DateTime TravelStart { get; set; }
        public DateTime TravelEnd { get; set; }
        public string RegistrationNumber { get; set; }
        public List<IFormFile> MediaFiles { get; set; } // For file uploads
        public string MediaDescription { get; set; } // Additional description
    }
}