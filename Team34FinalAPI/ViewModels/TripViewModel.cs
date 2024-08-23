namespace Team34FinalAPI.ViewModels
{
    public class TripViewModel
    {
        public int TripId { get; set; } // Required for identifying the trip to update
        public int BookingID { get; set; }
        public int? PreChecklistId { get; set; }
        public string Name { get; set; } // Replace VehicleId with Name
        public string Location { get; set; }
    
        public string Comment { get; set; }
        public DateTime TravelStart { get; set; }
        public DateTime TravelEnd { get; set; }
       
      
     
    }
}
