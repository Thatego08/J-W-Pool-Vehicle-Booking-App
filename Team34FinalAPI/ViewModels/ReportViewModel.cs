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
    }
}
