using static Team34FinalAPI.Report_DTO_s.ReportData;
using Team34FinalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.ViewModels;
using static Team34FinalAPI.ViewModels.ReportViewModel;

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

    }
    public class ReportService:IReportService
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
            var tripsWithAccidents = await _tripDbContext.Trips.CountAsync(t => t.HasAccidents);

            return new TripReportDto
            {
                TotalTrips = totalTrips,
                TripsWithAccidents = tripsWithAccidents
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
                    FuelCost = g.Sum(t => t.FuelAmount)
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

       

    }

}
