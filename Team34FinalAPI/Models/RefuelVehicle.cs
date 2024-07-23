using System.Text.Json.Serialization;

namespace Team34FinalAPI.Models
{
    public class RefuelVehicle
    {
        public int RefuelVehicleId { get; set; }
        public int TripId { get; set; }  // Foreign key to Trip

        // RefuelVehicle specific fields
        public string RadiatorWaterLevel { get; set; }
        public string Battery { get; set; }
        public string OilLevel { get; set; }
        public string BrakeFluidLevel { get; set; }
        public string ClutchFluidLevel { get; set; }
        public string WindowWasherFluidLevel { get; set; }
        public string VBeltCondition { get; set; }
        public string TyrePressure { get; set; }
        public string TyreCondition { get; set; }
        public string SpareWheelCondition { get; set; }
        public string Comments { get; set; }

        // Navigation property
        [JsonIgnore]
        public Trip Trip { get; set; }
    }
}
