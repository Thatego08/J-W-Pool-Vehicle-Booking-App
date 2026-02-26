using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Team34FinalAPI.ViewModels
{
    public class PostCheckViewModel
    {
        public decimal ClosingKms { get; set; }
        public string OilLeaks { get; set; }
        public string FuelLevel { get; set; }
        public string Mirrors { get; set; }
        public string SunVisor { get; set; }
        public string SeatBelts { get; set; }
        public string HeadLights { get; set; }
        public string Indicators { get; set; }
        public string ParkLights { get; set; }
        public string BrakeLights { get; set; }
        public string StrobeLight { get; set; }
        public string ReverseLight { get; set; }
        public string ReverseHooter { get; set; }
        public string Horn { get; set; }
        public string WindscreenWiper { get; set; }
        public string TyreCondition { get; set; }
        public string SpareWheelPresent { get; set; }
        public string JackAndWheelSpannerPresent { get; set; }
        public string Brakes { get; set; }
        public string Handbrake { get; set; }
        public DateTime? TravelEnd { get; set; }
        public string JWMarketingMagnets { get; set; }
        public string CheckedByJWSecurity { get; set; }
        public string LicenseDiskValid { get; set; }
        public string Comments { get; set; }
        public string AdditionalComments { get; set; }

        // Fields for TripMedia
        public int TripId { get; set; } // Add this property
        public List<IFormFile>? MediaFiles { get; set; }
        public string? MediaDescription { get; set; }
    }
}
