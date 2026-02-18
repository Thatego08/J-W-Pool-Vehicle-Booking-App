namespace Team34FinalAPI.Models
{
    public class LicenseDisk
    {
        public int Id { get; set; }

        public int VehicleID { get; set; }

        //public string VehicleName { get; set; }

        //public string VehicleRegistration { get; set; }

        public DateTime LicenseExpiryDate { get; set; }

        public string Status
        {
            get
            {
                return LicenseExpiryDate < DateTime.Today ? "Expired" : "Valid";
            }
        }
    }

}
