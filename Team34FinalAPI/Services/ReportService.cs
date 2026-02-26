using static Team34FinalAPI.Report_DTO_s.ReportData;
using Team34FinalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.ViewModels;
using static Team34FinalAPI.ViewModels.ReportViewModel;
using Team34FinalAPI.Report_DTO_s;
using System.Globalization;

namespace Team34FinalAPI.Services
{

    public interface IReportService
    {
        Task<IEnumerable<ReportViewModel.VehicleStatusReportViewModel>> GetVehicleStatusReportAsync();
        Task<IEnumerable<ReportViewModel.BookingTypeReportViewModel>> GetBookingTypeReportAsync();
        Task<IEnumerable<ReportViewModel.TripReportViewModel>> GetTripReportAsync();
        Task<IEnumerable<ReportViewModel.BookingStatusReportViewModel>> GetBookingStatusReportAsync();
        Task<IEnumerable<ReportViewModel.ProjectStatusReportViewModel>> GetProjectStatusReportAsync();

        Task<IEnumerable<ReportViewModel.VehicleMakeReportViewModel>> GetVehicleMakeReportAsync();
        Task<IEnumerable<FuelExpenditureReport>> GetFuelExpenditureReportAsync();
        Task<IEnumerable<BookingStatusReportViewModel>> GetFilteredBookingStatusReportAsync(string bookingType, DateTime? startDate, DateTime? endDate);
        Task<List<ProjectReportDto>> GetFilteredProjectsAsync(string projectStatus);
        Task<List<BookingPerUserReportViewModel>> GetBookingsPerUserPerMonthAsync();
        Task<List<CancelledBookingReportViewModel>> GetCancelledBookingsPerMonthAsync();
        //Task<List<AvailableVehiclesReportViewModel>> GetAvailableVehiclesForMonthAsync();
        Task<IEnumerable<TripDurationReportViewModel>> GetTripDurationReportAsync();





    }
    public class ReportService : IReportService
    {
        private readonly VehicleDbContext _vehicleDbContext;
        private readonly BookingDbContext _bookingDbContext;
        private readonly TripDbContext _tripDbContext;

        public ReportService(VehicleDbContext vehicleDbContext, BookingDbContext bookingDbContext, TripDbContext tripDbContext)
        {
            _vehicleDbContext = vehicleDbContext;
            _bookingDbContext = bookingDbContext;
            _tripDbContext = tripDbContext;
        }

        public async Task<IEnumerable<VehicleStatusReportViewModel>> GetVehicleStatusReportAsync()
        {
            return await _vehicleDbContext.Vehicles
                .GroupBy(v => v.Status.Name)
                .Select(g => new VehicleStatusReportViewModel
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingTypeReportViewModel>> GetBookingTypeReportAsync()
        {
            return await _bookingDbContext.Bookings
                .GroupBy(b => b.Type)
                .Select(g => new BookingTypeReportViewModel
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TripReportViewModel>> GetTripReportAsync()
        {
            return await _tripDbContext.Trips
                .GroupBy(t => t.Name)
                .Select(g => new TripReportViewModel
                {
                    TripType = g.Key,
                    Count = g.Count(),/*
                    Accidents = g.Sum(t => t.HasAccidents)*/
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingStatusReportViewModel>> GetBookingStatusReportAsync()
        {
            return await _bookingDbContext.Bookings
                .GroupBy(b => b.Status.Name)
                .Select(g => new BookingStatusReportViewModel
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReportData.VehicleFuelReportDto>> GetFuelExpendituresReportAsync()
        {
            return await _tripDbContext.Trips
                .SelectMany(t => t.RefuelVehicles, (t, rv) => new ReportData.VehicleFuelReportDto
                {
                    VehicleName = t.Name,            // Fetching the Name property from Trip
                    TripDate = t.TravelStart,        // Assuming TravelStart is the trip date
                    FuelAmount = rv.FuelQuantity,    // Fetching FuelQuantity from RefuelVehicle
                    FuelCost = rv.FuelCost           // Fetching FuelCost from RefuelVehicle
                })
                .ToListAsync();
        }




        public async Task<IEnumerable<ProjectStatusReportViewModel>> GetProjectStatusReportAsync()
        {
            return await _bookingDbContext.Projects
                .GroupBy(p => p.Status.Name)
                .Select(g => new ProjectStatusReportViewModel
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<VehicleReportDto>> GetVehicleReport()
        {
            return await _vehicleDbContext.Vehicles
                .GroupBy(v => v.Status)
                .Select(g => new VehicleReportDto
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToListAsync();
        }
        public async Task<IEnumerable<VehicleMakeReportViewModel>> GetVehicleMakeReportAsync()
        {
            return await _vehicleDbContext.Vehicles
                .GroupBy(v => v.VehicleMake.Name)
                .Select(g => new VehicleMakeReportViewModel
                {
                    Make = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }



        public async Task<List<BookingTypeReportDto>> GetBookingTypeReport()
        {
            return await _bookingDbContext.Bookings
                .GroupBy(b => b.Type)
                .Select(g => new BookingTypeReportDto
                {
                    Type = g.Key,
                    Count = g.Count()
                }).ToListAsync();
        }

        public async Task<TripReportDto> GetTripReport()
        {
            var totalTrips = await _tripDbContext.Trips.CountAsync();
            // var tripsWithAccidents = await _tripDbContext.Trips.CountAsync(t => t.HasAccidents);

            return new TripReportDto
            {
                TotalTrips = totalTrips,
                //TripsWithAccidents = tripsWithAccidents
            };
        }
        public async Task<IEnumerable<FuelExpenditureReport>> GetFuelExpenditureReportAsync()
        {
            return await _tripDbContext.Trips
                .GroupBy(t => t.Name)
                .Select(g => new FuelExpenditureReport
                {
                    Vehicle = g.Key,
                    TripCount = g.Count(),
                    //FuelCost = g.Sum(t => t.FuelAmount)
                })
                .ToListAsync();
        }


        public async Task<List<BookingStatusReportDto>> GetBookingStatusReport()
        {
            return await _bookingDbContext.Bookings
                .GroupBy(b => b.Status)
                .Select(g => new BookingStatusReportDto
                {
                    Status = g.Key,
                    Count = g.Count(),

                }).ToListAsync();
        }

        public async Task<List<ProjectReportDto>> GetProjectReport()
        {
            return await _bookingDbContext.Projects
                .GroupBy(p => p.Status)
                .Select(g => new ProjectReportDto
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToListAsync();
        }

        public async Task<IEnumerable<BookingStatusReportViewModel>> GetFilteredBookingStatusReportAsync(string bookingType, DateTime? startDate, DateTime? endDate)
        {
            var query = _bookingDbContext.Bookings.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(bookingType))
            {
                query = query.Where(b => b.Type == bookingType);
            }



            if (startDate.HasValue)
            {
                query = query.Where(b => b.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(b => b.EndDate <= endDate.Value);
            }

            // Group and select the report data
            return await query
                .GroupBy(b => b.Status.Name)
                .Select(g => new BookingStatusReportViewModel
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<ProjectReportDto>> GetFilteredProjectsAsync(string projectStatus)
        {
            // Start with the base query
            var query = _bookingDbContext.Projects.AsQueryable();

            // Apply filters based on provided parameters


            if (!string.IsNullOrEmpty(projectStatus))
            {
                query = query.Where(p => p.Status.Name == projectStatus);
            }


            // Group the results by project status and project the results into a DTO
            return await query
                .GroupBy(p => p.Status)
                .Select(g => new ProjectReportDto
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<UserTripReportDto>> GetTripsPerUserPerMonthAsync()
        {
            var trips = await _tripDbContext.Trips
           .GroupBy(t => new
           {
               t.UserName,
               Month = t.TravelStart.Month,  // Keep Month as int
               Year = t.TravelStart.Year
           })
           .Select(g => new
           {
               UserName = g.Key.UserName,
               Month = g.Key.Month,
               Year = g.Key.Year,
               TripCount = g.Count()
           })
           .ToListAsync(); // Perform the query and get the result from the database

            // Now convert the month number to month name in memory (client-side)
            return trips
        .Select(g => new UserTripReportDto
        {
            UserName = g.UserName,
            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Month), // Convert month int to name
            Year = g.Year,
            TripCount = g.TripCount
        })
        .ToList(); // This is now a synchronous call
        }


        public async Task<List<BookingPerUserReportViewModel>> GetBookingsPerUserPerMonthAsync()
        {
            var bookings = await _bookingDbContext.Bookings
                .GroupBy(b => new
                {
                    b.UserName,
                    Month = b.StartDate.Month,
                    Year = b.StartDate.Year
                })
                .Select(g => new BookingPerUserReportViewModel
                {
                    UserName = g.Key.UserName,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    BookingCount = g.Count()
                })
                .ToListAsync();

            return bookings;
        }


        public async Task<List<CancelledBookingReportViewModel>> GetCancelledBookingsPerMonthAsync()
        {
            var cancelledBookings = await _bookingDbContext.Bookings
                .Where(b => b.Status.Name == "Cancelled")
                .Select(b => new CancelledBookingReportViewModel
                {
                    UserName = b.UserName,
                    BookingId = b.BookingID,
                    CancelledDate = b.Status.DateChanged, // Access DateChanged
                })
                .ToListAsync();

            return cancelledBookings;
        }


        public async Task<IEnumerable<TripDurationReportViewModel>> GetTripDurationReportAsync()
        {
            var report = await (
                from trip in _tripDbContext.Trips
                join pre in _tripDbContext.PreChecklists on trip.PreChecklistId equals pre.Id into preGroup
                from pre in preGroup.DefaultIfEmpty()

                join post in _tripDbContext.PostChecks on trip.TripId equals post.TripId into postGroup
                from post in postGroup.DefaultIfEmpty()

                join booking in _tripDbContext.Bookings on trip.BookingID equals booking.BookingID

                join project in _tripDbContext.Projects on booking.ProjectId equals project.ProjectID into projectGroup
                from project in projectGroup.DefaultIfEmpty()


                select new TripDurationReportViewModel
                {
                    TripId = trip.TripId,
                    VehicleName = trip.Name,
                    Location = trip.Location,

                    BookingStart = booking.StartDate,
                    BookingEnd = booking.EndDate,

                    TravelStart = trip.TravelStart,
                    TravelEnd = trip.TravelEnd, // nullable is fine

                    EarliestStart = booking.StartDate < trip.TravelStart
                ? booking.StartDate
                : trip.TravelStart,

                    Duration = trip.TravelEnd.HasValue
           ? (trip.TravelEnd.Value.Date - trip.TravelStart.Date).Days + 1
           : 0, // or handle as needed if TravelEnd is TravelEnd



            OpeningKms = pre != null ? pre.OpeningKms : (decimal?)null,
                    ClosingKms = post != null ? post.ClosingKms : (decimal?)null,
                    TravelledKms = (pre != null && post != null)
                        ? (decimal?)(post.ClosingKms - pre.OpeningKms)
                        : null,

                    ProjectNumber = project != null ? project.ProjectNumber : (int?)null
                }
            ).ToListAsync();

            return report;
        }





        //public async Task<List<AvailableVehiclesReportViewModel>> GetAvailableVehiclesForMonthAsync()
        //{
        //    var availableVehicles = await _vehicleDbContext.Vehicles
        //        .Where(v => v.Status.Name == "Available") // Example filter
        //        .GroupBy(v => new
        //        {
        //            Month = v.CreatedDate.Month, // Ensure CreatedDate is the correct field
        //            Year = v.CreatedDate.Year
        //        })
        //        .Select(g => new AvailableVehiclesReportViewModel
        //        {
        //            Month = g.Key.Month,
        //            Year = g.Key.Year,
        //            AvailableCount = g.Count()
        //        })
        //        .ToListAsync();

        //    return availableVehicles;
        //}


    }

}