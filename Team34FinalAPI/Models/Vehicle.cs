using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team34FinalAPI.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateAcquired { get; set; }

        public DateTime LicenseExpiryDate { get; set; }

        public string RegistrationNumber { get; set; }

        public int InsuranceCoverID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string VehicleType { get; set; }

        

        public string VIN { get; set; }

        public string EngineNo {  get; set; }

        public int ColourID { get; set; }   

        public int FuelTypeID { get; set; }

        public int StatusID { get; set; }
        public int VehicleMakeID { get;  set; }

        public int VehicleModelID { get; set; }

        // This line establishes the one-to-many relationship
        public ICollection<Trip> Trips { get; set; }
        
        public ICollection<Booking> Bookings { get; set; }

        public LicenseDisk LicenseDisk { get; set; }
       
        public InsuranceCover InsuranceCover { get; set; }
        public Colour Colour { get; set; }
        public VehicleFuelType FuelType { get; set; }
        public VehicleMake VehicleMake { get; set; }
        public  VehicleModel VehicleModel { get; set; }

        public Status? Status { get; set; }

        //Salvage additions
       
        public string CabinType { get; set; }      // 'Double', 'Single', 'Extra', 'Hatch', 'SUV'
        public string DriveType { get; set; }      // '4x4', '4x2'
        public string Transmission { get; set; }   // 'Manual', 'Automatic'
        public bool HasTowBar { get; set; }
        public bool HasCanopy { get; set; }

        public string? Compliance { get; set; }
        public string? Protection { get; set; }


    }
}
