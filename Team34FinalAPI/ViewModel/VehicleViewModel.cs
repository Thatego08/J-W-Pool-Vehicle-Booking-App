namespace Team34FinalAPI.ViewModel
{
    public class VehicleViewModel
    {
        public string Name { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }
        public int VehicleModelID { get; set; }
        public int VehicleMakeID { get; set; }
        public DateTime DateAcquired { get; set; }

        public string RegistrationNumber { get; set; }

        public string EngineNo { get; set; }

        public string VIN { get; set; }

        public int InsuranceID { get; set; }

        public int ColourID { get; set; }

        public int FuelTypeID { get; set; }

        public int StatusID { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
    }
}
