namespace Team34FinalAPI.ViewModels
{
    public class BookingViewModel
    {
        public int BookingID { get; set; }
        public string UserName { get; set; }
        public string? Event { get; set; }
        public string? Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VehicleName { get; set; }

        public string VehicleRegistration { get; set; }
        public int? ProjectNumber { get; set; }

    }
}
