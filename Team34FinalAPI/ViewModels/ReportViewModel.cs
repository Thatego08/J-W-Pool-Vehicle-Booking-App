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
