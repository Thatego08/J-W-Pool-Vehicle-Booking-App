namespace Team34FinalAPI.ViewModels
{
    public class VehicleViewModel
    {
        public int VehicleID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int VehicleMakeID { get; set; }

        public int VehicleModelID { get; set; }

        public DateTime DateAcquired { get; set; }

        public DateTime LicenseExpiryDate { get; set; }

        public string RegistrationNumber { get; set; }

        public int InsuranceCoverID { get; set; }

        public string VIN { get; set; }

        public string EngineNo { get; set; }

        public int ColourID { get; set; }

        public int FuelTypeID { get; set; }

        public int StatusID { get; set; }
    }
}
