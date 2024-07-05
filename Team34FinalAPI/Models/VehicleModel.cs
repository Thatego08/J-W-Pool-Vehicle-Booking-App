namespace Team34FinalAPI.Models
{
    public class VehicleModel
    {
        public int VehicleModelID { get; set; }

        public string VehicleModelName { get; set; }

        public int VehicleMakeID { get; set; }

        public VehicleMake VehicleMake { get; set; }
    }
}
