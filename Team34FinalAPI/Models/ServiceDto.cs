namespace Team34FinalAPI.Models
{
    public class ServiceDto
    {
        public int ServiceID { get; set; }
        public int VehicleID { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public string VehicleModelName { get; set; }
        public string VehicleMakeName { get; set; }
        public string Description { get; set; }
        public DateTime ServiceDate { get; set; }
    }
}
