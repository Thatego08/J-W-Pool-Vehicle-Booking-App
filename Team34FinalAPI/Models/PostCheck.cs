using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;


namespace Team34FinalAPI.Models
{
    public class PostCheck
    {
        [Key]
        public int PostCheckId { get; set; }

        // Foreign key property for Trip
        public int TripId { get; set; }
        [ForeignKey(nameof(TripId))]  // <-- Explicit FK configuration
        [JsonIgnore] // Avoid circular references in JSON serialization
      
        public Trip Trip { get; set; } // Navigation property
        public decimal ClosingKms { get; set; }
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

       

        public string Comments { get; set; }
        public string AdditionalComments { get; set; }

        public ICollection<TripMedia> TripMedia { get; set; } = new List<TripMedia>();
    }
}
