namespace Team34FinalAPI.ViewModels
{
    public class LicenseDiskViewModel
    {
        public int Id { get; set; }
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string VehicleRegistration { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string Status { get; set; }
    }
}
