using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team34FinalAPI.Models
{
    public class PreChecklist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal OpeningKms { get; set; }

        public bool OilLeaks { get; set; }
        public bool FuelLevel { get; set; }
        public bool Mirrors { get; set; }
        public bool SunVisor { get; set; }
        public bool SeatBelts { get; set; }
        public bool HeadLights { get; set; }
        public bool Indicators { get; set; }
        public bool ParkLights { get; set; }
        public bool BrakeLights { get; set; }
        public bool StrobeLight { get; set; }
        public bool ReverseLight { get; set; }
        public bool ReverseHooter { get; set; }
        public bool Horn { get; set; }
        public bool WindscreenWiper { get; set; }
        public bool TyreCondition { get; set; }
        public bool SpareWheelPresent { get; set; }
        public bool JackAndWheelSpannerPresent { get; set; }
        public bool Brakes { get; set; }
        public bool Handbrake { get; set; }
        public bool JWMarketingMagnets { get; set; }
        public bool CheckedByJWSecurity { get; set; }
        public bool LicenseDiskValid { get; set; }

        public string Comments { get; set; } // Optional comments field
        public string AdditionalComments { get; set; } // Additional comments field if needed

        // Foreign key for Booking
        public int BookingID { get; set; }

        [ForeignKey("BookingID")]
        [JsonIgnore]
        public Booking? Booking { get; set; }
    }
}
