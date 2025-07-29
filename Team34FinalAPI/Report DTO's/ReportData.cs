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

        public class FuelExpenditureReport
        {
            public string Vehicle { get; set; }
            public int TripCount { get; set; }
            public decimal FuelCost { get; set; }
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
        public class VehicleFuelReportDto
        {
            public string VehicleName { get; set; }
            public DateTime TripDate { get; set; }
            public decimal FuelAmount { get; set; }
            public decimal FuelCost { get; set; }
        }

        public class UserTripReportDto
        {
            public string UserName { get; set; }
            public string Month { get; set; }     // Change from int to string  // Month number (1 for January, 12 for December)
            public int Year { get; set; }   // Year of the trips
            public int TripCount { get; set; }  // Count of trips for that user in the specific month
        }


        //Count of bookings a user made in the specific month
        public class UserBookingReportDto 
        {
            public string UserName { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public int BookingCount { get; set; } 
        }

        //Number of cancelled bookings per month
        public class CancelledBookingReportDto
        {
            public string UserName { get; set; } 
            public int BookingID { get; set; }   
            public DateTime CancelledDate { get; set; } 
        }

        public class TripDurationReportDto
        {
            public int TripId { get; set; }
            public string VehicleName { get; set; }
            public string Location { get; set; }
            public decimal? OpeningKms { get; set; }  // from PreChecklist, nullable in case no PreChecklist
            public decimal? ClosingKms { get; set; }
            public TimeSpan Duration { get; set; }    // calculated as TravelEnd - TravelStart
            public DateTime TravelStart { get; set; }
            public DateTime TravelEnd { get; set; }
            public DateTime EarliestStart { get; set; }
            public int? ProjectNumber { get; set; }
            public DateTime BookingStart { get; set; } // booking.StartDate
            public DateTime BookingEnd { get; set; }   // booking.EndDate
        }


        //Available vehicles for the month
        //public class AvailableVehicleReportDto
        //{
        //    public string VehicleName { get; set; }
        //    public int Month { get; set; }
        //    public int Year { get; set; }
        //    public int AvailableDays { get; set; }
        //}


    }
}
