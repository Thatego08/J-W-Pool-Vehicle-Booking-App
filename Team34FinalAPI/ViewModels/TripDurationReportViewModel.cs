using System;

namespace Team34FinalAPI.ViewModels
{
    public class TripDurationReportViewModel
    {
        public int TripId { get; set; }
        public string VehicleName { get; set; }
        public string Location { get; set; }

        public DateTime BookingStart { get; set; }
        public DateTime BookingEnd { get; set; }

        public DateTime TravelStart { get; set; }
        public DateTime? TravelEnd { get; set; }

        public DateTime EarliestStart { get; set; }

        public int Duration { get; set; } // In days

        public decimal? OpeningKms { get; set; }
        public decimal? ClosingKms { get; set; }
        public decimal? TravelledKms { get; set; }

        public int? ProjectNumber { get; set; }
    }
}
