namespace Team34FinalAPI.ViewModels
{
    public class ReportViewModel
    {
        public class VehicleStatusReportViewModel
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public class BookingTypeReportViewModel
        {
            public string Type { get; set; }
            public int Count { get; set; }
        }

        public class TripReportViewModel
        {
            public int TotalTrips { get; set; }
            public int TripsWithAccidents { get; set; }

            public string TripType { get; set; }
            public int Count { get; set; }
            public int Accidents { get; set; }
        }

        public class FuelExpendituresReportViewModel
        {
            public string VehicleName { get; set; }
            public DateTime TripDate { get; set; }
            public decimal FuelAmount { get; set; }
            public decimal FuelCost { get; set; }
        }
        public class FuelExpenditureReportViewModel
        {
            public string Vehicle { get; set; }
            public int TripCount { get; set; }
            public decimal FuelCost { get; set; }
        }

        public class BookingStatusReportViewModel
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public class TripCostingReportViewModel
        {
            public DateTime CalculateStart { get; set; }   // Booking StartDate
            public DateTime CalculateEnd { get; set; }     // TravelEnd from Trip

            // Number of days for costing (end - start + 1)
            public int Days { get; set; }

            // Rate information
            public decimal Rate { get; set; }              // Fixed daily rate
            public decimal Amount { get; set; }
            public int TripId { get; set; } // Days * Rate
            public int? ProjectId { get; set; }
            public string VehicleName { get; set; } // optional
            public string Location { get; set; }    // optional

            // Travel km information
            public decimal? TravelKms { get; set; }        // ClosingKms - OpeningKms
            public decimal TravelKmRate { get; set; }      // Fixed rate per km
            public decimal? TravelKmAmount { get; set; }    // TravelKms * TravelKmRate

            // Total sum (Amount + TravelKmAmount)
            public decimal? TotalSum { get; set; }
        }

        public class ProjectStatusReportViewModel
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }
        
        public class VehicleMakeReportViewModel
        {
            public string Make { get; set; }
            public int Count { get; set; }
        }


        public class BookingPerUserReportViewModel
        {
            public string UserName { get; set; }
            public int Month { get; set; }
            public int Year { get; set; } // Add this property
            public int BookingCount { get; set; }
        }

        public class CancelledBookingReportViewModel
        {
            public string UserName { get; set; }
            public int BookingId { get; set; }
            public DateTime CancelledDate { get; set; } // We'll use the booking's EndDate as a proxy
            
        }


        //public class AvailableVehiclesReportViewModel
        //{
        //    public string VehicleName { get; set; }
        //    public int Month { get; set; } // Add this property
        //    public int Year { get; set; }  // Add this property
        //    public int AvailableCount { get; set; }
        //}

    }
}
