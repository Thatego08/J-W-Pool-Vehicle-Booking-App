using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Schema;

namespace Team34FinalAPI.Models
{
    public class VehicleChecklist
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } //foreign key
        public Vehicle Vehicle { get; set; }//navigation property
        public string RegistrationNumber { get; set; }

        public int VehicleMakeId { get; set; } // Foreign key
        public VehicleMake VehicleMake { get; set; } // Navigation property

        public int VehicleModelId { get; set; } // Foreign key
        public VehicleModel VehicleModel { get; set; } // Navigation property

        public decimal OpeningKms { get; set; }
        public decimal ClosingKms {  get; set; }

        [NotMapped]
        public decimal DistanceTravelled
        {
            get
            {
                return ClosingKms - OpeningKms;
            }
        }
        public ExteriorChecks ExteriorChecks { get; set; }

        public InteriorChecks InteriorChecks { get; set; }

        public UnderTheHoodChecks UnderTheHoodChecks { get; set; }

        public FunctionalTests FunctionalTests { get; set; }

        public SafetyEquipment SafetyEquipment { get; set; }

        public Documentation Documentation { get; set; }

    }

    public class ExteriorChecks
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


    public class InteriorChecks
    {
        public bool Horn { get; set; }
        public bool Seatbelt { get; set; }
        public bool SunVisor { get; set; }
        public bool Sunblock { get; set; }
        public bool Windscreen { get; set; }
    }

    public class UnderTheHoodChecks
    {
        public bool FuelLevel { get; set; }
    }

    public class FunctionalTests
    {
        public bool Indicator { get; set; }
        public bool ReverseHooter { get; set; }
        public bool Brakes { get; set; }
        public bool Handbrake { get; set; }
    }

    public class SafetyEquipment
    {
        public bool FireExtinguisher { get; set; }
        public bool InspectionValid { get; set; }
        public bool TriangleInPlace3x { get; set; }
        public bool JackWheelPresent { get; set; }
    }

    public class Documentation
    {
        public bool LicenseDisks { get; set; }
        public bool CheckedBySecurity { get; set; }
    }

}