using Team34FinalAPI.Models;

namespace Team34FinalAPI.Report_DTO_s
{
    public class ReportData
    {
        public class VehicleReportDto
        {
            public Status Status { get; set; }
            public int Count { get; set; }
        }

        public class BookingTypeReportDto
        {
            public string Type { get; set; }
            public int Count { get; set; }
        }

        public class TripReportDto
        {
            public int TotalTrips { get; set; }
            public int TripsWithAccidents { get; set; }

            public string TripType { get; set; }
            public int Count { get; set; }
            public int Accidents { get; set; }
        }

        public class BookingStatusReportDto
        {
            public Status Status { get; set; }
            public int Count { get; set; }
        }

        public class ProjectReportDto
        {
            public Status Status { get; set; }
            public int Count { get; set; }
        }


    }
}
