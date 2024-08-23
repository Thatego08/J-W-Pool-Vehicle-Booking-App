using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Schema;

namespace Team34FinalAPI.Models
{
    public class PostChecklist
    {

        public int PostId { get; set; }
        public int VehicleId { get; set; } //foreign key

        public Vehicle Vehicle { get; set; }   
        public string UserName { get; set; }

        public bool ReturnVehicle { get; set; }   //checklist will only be submitted if this has been checked regardless of other checkboxes 


        public decimal OpeningKms { get; set; }
        public decimal ClosingKms { get; set; }

        [NotMapped]
        public decimal DistanceTravelled
        {
            get
            {
                return ClosingKms - OpeningKms;
            }
        }
        [NotMapped]
        public PostExteriorChecks ExteriorChecks { get; set; }
        [NotMapped]
        public PostInteriorChecks InteriorChecks { get; set; }
        [NotMapped]
        public PostUnderTheHoodChecks UnderTheHoodChecks { get; set; }
        [NotMapped]
        public PostFunctionalTests FunctionalTests { get; set; }
        [NotMapped]
        public PostSafetyEquipment SafetyEquipment { get; set; }
        [NotMapped]
        public PostDocumentation Documentation { get; set; }

    }

    public class PostExteriorChecks
    {
        public bool Mirrors { get; set; }
        public bool OilWaterLeaks { get; set; }
        public bool HeadLights { get; set; }
        public bool ParkLights { get; set; }
        public bool BrakeLights { get; set; }
        public bool StrobeLights { get; set; }
        public bool ReverseLight { get; set; }
        public bool TyreCondition { get; set; }
        public bool SpareWheelPresent { get; set; }
        public bool DamageToInteriorBodywork { get; set; }
        public bool MarketingMagnets { get; set; }
    }


    public class PostInteriorChecks
    {
        public bool Horn { get; set; }
        public bool Seatbelt { get; set; }
        public bool SunVisor { get; set; }
        public bool Sunblock { get; set; }
        public bool Windscreen { get; set; }
    }

    public class PostUnderTheHoodChecks
    {
        public bool FuelLevel { get; set; }
    }

    public class PostFunctionalTests
    {
        public bool Indicator { get; set; }
        public bool ReverseHooter { get; set; }
        public bool Brakes { get; set; }
        public bool Handbrake { get; set; }
    }

    public class PostSafetyEquipment
    {
        public bool FireExtinguisher { get; set; }
        public bool InspectionValid { get; set; }
        public bool TriangleInPlace3x { get; set; }
        public bool JackWheelPresent { get; set; }
    }
    [NotMapped]
    public class PostDocumentation
    {
        public bool LicenseDisks { get; set; }
        public bool CheckedBySecurity { get; set; }
    }

}

