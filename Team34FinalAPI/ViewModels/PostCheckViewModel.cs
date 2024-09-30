using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Team34FinalAPI.ViewModels
{
    public class PostCheckViewModel
    {
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

        // Fields for TripMedia
        public int TripId { get; set; } // Add this property
        public List<IFormFile>? MediaFiles { get; set; }
        public string? MediaDescription { get; set; }
    }
}
