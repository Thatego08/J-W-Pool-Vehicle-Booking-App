using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.VisualBasic.FileIO;
using Team34FinalAPI.Models;


namespace Team34FinalAPI.Models
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
        : base(options)
        {
        } 
        public DbSet<Colour> Colour { get; set; }

        public DbSet<InsuranceCover> InsuranceCover { get; set; }
        public DbSet<LicenseDisk> LicenseDisks { get; set; }

        public DbSet<Status> Status { get; set; }
        public DbSet<VehicleFuelType> FuelTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleMake> VehicleMake { get; set; }
        public DbSet<VehicleModel> VehicleModel { get; set; }
        public DbSet<Service> VehicleService { get; set; }
        public DbSet<ServiceHistory> ServiceHistory { get; set; }

        public DbSet<VehicleChecklist> VehicleChecklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Colour>().HasData(
                new Colour { Id = 1, Name = " Black" },
                new Colour { Id = 2, Name = " White " },
                new Colour { Id = 3, Name = " Blue" },
                new Colour { Id = 4, Name = "Green" },
                new Colour { Id = 5, Name = " Grey" },
                new Colour { Id = 6, Name = "Red" },
            new Colour { Id = 7, Name = " Silver" }
            );

            modelBuilder.Entity<VehicleFuelType>().HasData(
                new VehicleFuelType { Id = 1, FuelName = "Petrol" },
                new VehicleFuelType { Id = 2, FuelName = "Diesel" }
                );

            modelBuilder.Entity<InsuranceCover>().HasData(

                new InsuranceCover { InsuranceCoverId = 1, InsuranceCoverName = " By Seun" },
                new InsuranceCover { InsuranceCoverId = 2, InsuranceCoverName = "Comprehensive " },
                new InsuranceCover { InsuranceCoverId = 3, InsuranceCoverName = "Third Party Only" },
                new InsuranceCover { InsuranceCoverId = 4, InsuranceCoverName = " Outsurance " });

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Available " },
                new Status { Id = 2, Name = " Booked " },
                new Status { Id = 3, Name = " In For Service " }
                );

            modelBuilder.Entity<LicenseDisk>().HasData(
                new LicenseDisk { Id = 1, VehicleID = 1, LicenseExpiryDate = new DateTime(2024, 12, 31) },
                new LicenseDisk { Id = 2, VehicleID = 2, LicenseExpiryDate = new DateTime(2023, 8, 31) },
                new LicenseDisk { Id = 3, VehicleID = 3, LicenseExpiryDate = new DateTime(2022, 11, 30) },
                new LicenseDisk { Id = 4, VehicleID = 4, LicenseExpiryDate = new DateTime(2028, 5, 31) },
                new LicenseDisk { Id = 5, VehicleID = 5, LicenseExpiryDate = new DateTime(2023, 8, 31) },
                new LicenseDisk { Id = 6, VehicleID = 6, LicenseExpiryDate = new DateTime(2023, 2, 28) },
                new LicenseDisk { Id = 7, VehicleID = 7, LicenseExpiryDate = new DateTime(2023, 10, 31) },
                new LicenseDisk { Id = 8, VehicleID = 8, LicenseExpiryDate = new DateTime(2023, 10, 31) },
                new LicenseDisk { Id = 9, VehicleID = 9, LicenseExpiryDate = new DateTime(2023, 10, 31) },
                new LicenseDisk { Id = 10, VehicleID = 10, LicenseExpiryDate = new DateTime(2023, 10, 31) },
                new LicenseDisk { Id = 11, VehicleID = 11, LicenseExpiryDate = new DateTime(2023, 8, 31) },
                new LicenseDisk { Id = 12, VehicleID = 12, LicenseExpiryDate = new DateTime(2023, 8, 31) },
                new LicenseDisk { Id = 13, VehicleID = 13, LicenseExpiryDate = new DateTime(2023, 8, 31) },
                new LicenseDisk { Id = 14, VehicleID = 14, LicenseExpiryDate = new DateTime(2024, 4, 30) },
                new LicenseDisk { Id = 15, VehicleID = 15, LicenseExpiryDate = new DateTime(2028, 10, 31) },
                new LicenseDisk { Id = 16, VehicleID = 16, LicenseExpiryDate = new DateTime(2023, 9, 30) },
                new LicenseDisk { Id = 17, VehicleID = 17, LicenseExpiryDate = new DateTime(2022, 8, 31) },
                new LicenseDisk { Id = 18, VehicleID = 18, LicenseExpiryDate = new DateTime(2028, 6, 1) },
                new LicenseDisk { Id = 19, VehicleID = 19, LicenseExpiryDate = new DateTime(2027, 8, 1) },
                new LicenseDisk { Id = 20, VehicleID = 20, LicenseExpiryDate = new DateTime(2028, 12, 14) }
            );

            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { VehicleID = 1, Name = "Hyundai 1", VehicleModelID = 1, VehicleMakeID = 7, Description = "i20", DateAcquired = new DateTime(2024, 2,16), RegistrationNumber = "LN 68 YM GP", InsuranceCoverID = 2, VIN = "MALBG512LPM254886", ColourID = 2, FuelTypeID = 1, EngineNo = "G4LFPV302509", LicenseExpiryDate = new DateTime(2024, 12, 31), StatusID = 1 },
                new Vehicle { VehicleID = 2, Name = "Toyota 1", VehicleModelID = 2, VehicleMakeID = 11, Description = "Extra Cab 2.4 GD-6 RB SRX 6MT ", DateAcquired = new DateTime(2019, 1,26 ), RegistrationNumber = "JF 72 WJ GP", InsuranceCoverID = 3, VIN = "AHTJB8DC404730166", ColourID = 2, FuelTypeID = 2, EngineNo = "2GDC598667", LicenseExpiryDate = new DateTime(2023, 8, 31), StatusID = 1},
                new Vehicle { VehicleID = 3, Name = "Toyota 2",VehicleModelID = 2, VehicleMakeID = 11, Description = "Extra Cab 2.4 GD-6 RB SRX 6MT", DateAcquired = new DateTime(2020, 12, 4), RegistrationNumber = "JT 99 LF GP", InsuranceCoverID = 2, VIN = "AHTJB3DCX04490835", ColourID = 2, FuelTypeID = 2, EngineNo = "2GDC765766", LicenseExpiryDate = new DateTime(2022, 11, 30), StatusID = 1 },
                new Vehicle { VehicleID = 4, Name = "Toyota 3",VehicleModelID = 2, VehicleMakeID = 11, Description = "DC HILUX 2.4 GD-6 RAIDER 4X4 A/T", DateAcquired = new DateTime(2024, 7, 24), RegistrationNumber = "LR 01 CT GP", InsuranceCoverID = 2, VIN = "AHTKB3CD802676867", ColourID = 2, FuelTypeID = 2, EngineNo = "2GDD364709", LicenseExpiryDate = new DateTime(2028, 5, 31), StatusID = 1 },
                new Vehicle { VehicleID = 5, Name = "Toyota 4", VehicleModelID = 2, VehicleMakeID = 11, Description = " SC 2.4 GD6 RB SRX MT (Z50) 2019", DateAcquired = new DateTime(2019, 3, 29), RegistrationNumber = "HY 06 DR GP", InsuranceCoverID = 3, VIN = "AHTJB8DB104579407", ColourID = 2, FuelTypeID = 2, EngineNo = "2GDC503748", LicenseExpiryDate = new DateTime(2023, 8, 31), StatusID = 1},
                new Vehicle { VehicleID = 6, Name = "Toyota 5", VehicleModelID = 2, VehicleMakeID = 11, Description = "DC, Raider 2.7 D, 4X4, ROPS ", DateAcquired = new DateTime(2012, 2, 24), RegistrationNumber = "BT 11 SL GP", InsuranceCoverID = 3, VIN = "AHTFR22G906054497", ColourID = 2, FuelTypeID = 1, EngineNo = "2KD5635798", LicenseExpiryDate = new DateTime(2028, 2, 28), StatusID = 1 },
                new Vehicle { VehicleID = 7, Name = "Toyota 6", VehicleModelID = 7, VehicleMakeID = 11, Description = "DC", DateAcquired = new DateTime(2005, 6, 30), RegistrationNumber = "WZV 941 GP", InsuranceCoverID = 3, VIN = "AHTEZ39G207010469", ColourID = 7, FuelTypeID = 7, EngineNo = "1KD7486383", LicenseExpiryDate = new DateTime(2023, 10, 31), StatusID = 6 },
                new Vehicle { VehicleID = 8, Name = "Toyota 7", VehicleModelID = 2, VehicleMakeID = 11, Description = "DC, 4X4 ( With Canopy ) ", DateAcquired = new DateTime(2016, 11, 18), RegistrationNumber = "FN 57 RR GP", InsuranceCoverID = 3, VIN = "AHTKB3CD302604992", ColourID = 2, FuelTypeID = 2, EngineNo = "2GD0195958", LicenseExpiryDate = new DateTime(2023, 10, 31), StatusID = 1},
                new Vehicle { VehicleID = 9, Name = "Toyota 8", VehicleModelID = 2, VehicleMakeID = 11, Description = "DC, 4x4", DateAcquired = new DateTime(2016, 11, 18), RegistrationNumber = "FN 57 ST GP", InsuranceCoverID = 3, VIN = "AHTK3CD202605213", ColourID = 2, FuelTypeID = 2, EngineNo = "2GD0207661", LicenseExpiryDate = new DateTime(2023, 10, 31), StatusID = 1 },
                new Vehicle { VehicleID = 10, Name = "Toyota 9",VehicleModelID = 2, VehicleMakeID = 11, Description = "2.8 GD-6DC, 4x4 Auto ( With Canopy ) ", DateAcquired = new DateTime(2017, 4, 11), RegistrationNumber = "FV 25 XR GP", InsuranceCoverID = 3, VIN = "AHTHA3CD403417011", ColourID = 2, FuelTypeID = 2, EngineNo = "1GD0252789", LicenseExpiryDate = new DateTime(2023, 10, 31), StatusID = 1 },
                new Vehicle { VehicleID = 11, Name = "Toyota 10",VehicleModelID = 2, VehicleMakeID = 11, Description = " SC 2.4 GD-6RB, SRX 4x2 ", DateAcquired = new DateTime(2017, 6, 22), RegistrationNumber = "FX 22 YD GP", InsuranceCoverID = 3, VIN = "AHTJB8DB504573626", ColourID = 2, FuelTypeID = 1, EngineNo = "2GD0284816", LicenseExpiryDate = new DateTime(2023, 8, 31), StatusID = 1 },
                new Vehicle { VehicleID = 12, Name = "Toyota 11", VehicleModelID = 2, VehicleMakeID = 11, Description = " SC 2.4 GD-6RB, SRX 4x2 ", DateAcquired = new DateTime(2017, 6, 22), RegistrationNumber = "FX 23 BX GP", InsuranceCoverID = 3, VIN = "AHTJB8DB304573608", ColourID = 2, FuelTypeID = 1, EngineNo = "2GD0284247", LicenseExpiryDate = new DateTime(2023, 8, 31), StatusID = 1},
                new Vehicle { VehicleID = 13, Name = "Toyota 12",VehicleModelID = 2, VehicleMakeID = 11, Description = " DC 2.4 GD-6RB SR MT ", DateAcquired = new DateTime(2022, 5, 27), RegistrationNumber = "KP 05 MC GP", InsuranceCoverID = 2, VIN = "AHTJB3DD504527511", ColourID = 2, FuelTypeID = 1, EngineNo = "2GDC910427", LicenseExpiryDate = new DateTime(2023, 8, 31), StatusID = 1 },
                new Vehicle { VehicleID = 14, Name = "Toyota 13", VehicleModelID = 2, VehicleMakeID = 11, Description = " DC 2.4 GD6 RB RAI MT ( With Canopy ) ", DateAcquired = new DateTime(2022, 11, 1), RegistrationNumber = "KW 51 TL GP", InsuranceCoverID = 2, VIN = "AHTJB3DD304529631", ColourID = 2, FuelTypeID = 2, EngineNo = "2GDD036238", LicenseExpiryDate = new DateTime(2024, 4, 30), StatusID = 1 },
                new Vehicle { VehicleID = 15, Name = "Toyota 14",VehicleModelID = 2, VehicleMakeID = 11, Description = " Extra Cab HiluxXC 2.4 GD6 RBRAI 6MT ( A1P )  ", DateAcquired = new DateTime(2023, 4, 1), RegistrationNumber = "LB 93 JV GP", InsuranceCoverID = 2, VIN = "AHTJB3DC104496782", ColourID = 2, FuelTypeID = 2, EngineNo = "2GDD138062", LicenseExpiryDate = new DateTime(2023, 10, 31), StatusID = 1},
                new Vehicle { VehicleID = 16, Name = "Toyota 15",VehicleModelID = 2, VehicleMakeID = 11, Description = "1.8 XR CVT ", DateAcquired = new DateTime(2022, 10, 19), RegistrationNumber = "KV 84 DV GP ", InsuranceCoverID = 2, VIN = "AHTKFBAG500614236", ColourID = 2, FuelTypeID = 2, EngineNo = "2ZRW975634", LicenseExpiryDate = new DateTime(2023, 9, 30), StatusID = 1 },
                new Vehicle { VehicleID = 17, Name = "Toyota S1",VehicleModelID = 2, VehicleMakeID = 11, Description = "DC 2.8 GD-6RB, 4x2", DateAcquired = new DateTime(2020, 1, 1), RegistrationNumber = " HH 91 VZ GP ", InsuranceCoverID = 1, VIN = "AHTGA3DD900968871", ColourID = 2, FuelTypeID = 2, EngineNo = "1GD0374210", LicenseExpiryDate = new DateTime(2022, 8, 31), StatusID = 1 },
                new Vehicle { VehicleID = 18, Name = "Toyota S2", VehicleModelID = 2, VehicleMakeID = 11, Description = " SC 2.4 GD-6RB, SRX 4x2 ", DateAcquired = new DateTime(2021, 1, 1), RegistrationNumber = " FV 69 WP GP ", InsuranceCoverID = 1, VIN = "", ColourID = 2, FuelTypeID = 2, EngineNo = "", LicenseExpiryDate = new DateTime(2028, 6, 1), StatusID = 1 },
                new Vehicle { VehicleID = 19, Name = "Toyota S3",VehicleModelID = 2, VehicleMakeID = 11, Description = " SC 2.4 GD-6RB, SRX 4x2", DateAcquired = new DateTime(2005, 7, 9), RegistrationNumber = "HC 85 FY GP", InsuranceCoverID = 1, VIN = "AHTJB8DBX04575081", ColourID = 2, FuelTypeID = 2, EngineNo = "2GD0329959", LicenseExpiryDate = new DateTime(2027, 8, 1), StatusID = 1 },
                new Vehicle { VehicleID = 20, Name = "Isuzu 1",VehicleModelID = 2, VehicleMakeID = 9, Description = " DC DMAX 2.2", DateAcquired = new DateTime(2021, 1, 1), RegistrationNumber = " ND 836-010", InsuranceCoverID = 1, VIN = "ACVNRCHR3K1070230", ColourID = 2, FuelTypeID = 1, EngineNo = "4JK1VT8141", LicenseExpiryDate = new DateTime(2028, 12, 14), StatusID = 1 }

            );


            modelBuilder.Entity<VehicleMake>().HasData(
                new VehicleMake { VehicleMakeID = 1, Name = "Audi" },
                new VehicleMake { VehicleMakeID = 2, Name = "Brandt BRV" },
                new VehicleMake { VehicleMakeID = 3, Name = "Citroen" },
                new VehicleMake { VehicleMakeID = 4, Name = "Ford" },
                new VehicleMake { VehicleMakeID = 5, Name = "Haval" },
                new VehicleMake { VehicleMakeID = 6, Name = "Honda" },
                new VehicleMake { VehicleMakeID = 7, Name = "Hyundai" },
                new VehicleMake { VehicleMakeID = 8, Name = "Kia" },
                new VehicleMake { VehicleMakeID = 9, Name = "Isuzu" },
                new VehicleMake { VehicleMakeID = 10, Name = "Nissan" },
                new VehicleMake { VehicleMakeID = 11, Name = "Toyota" }
                );

            modelBuilder.Entity<VehicleModel>().HasData(
                new VehicleModel { VehicleModelID = 1, VehicleModelName = " Corolla Cross", VehicleMakeID = 11 },
                new VehicleModel { VehicleModelID = 2, VehicleModelName = " Hilux ", VehicleMakeID = 11 },
                new VehicleModel { VehicleModelID = 3, VehicleModelName = " Izuzu", VehicleMakeID = 9 },
                new VehicleModel { VehicleModelID = 4, VehicleModelName = " A3", VehicleMakeID = 1 },
                new VehicleModel { VehicleModelID = 5, VehicleModelName = " A4 ", VehicleMakeID = 1 },
                new VehicleModel { VehicleModelID = 6, VehicleModelName = " Grand i10", VehicleMakeID = 7 },
                new VehicleModel { VehicleModelID = 7, VehicleModelName = " Venue ", VehicleMakeID = 7 },
                new VehicleModel { VehicleModelID = 8, VehicleModelName = " TUCSON", VehicleMakeID = 7 },
                new VehicleModel { VehicleModelID = 9, VehicleModelName = " Ranger", VehicleMakeID = 4 }
                );



            modelBuilder.Entity<Service>().HasData(
                new Service { ServiceID = 1, VehicleID = 1, Description = " The brake was not working", ServiceDate = new DateTime(2022, 1, 1) },
                new Service { ServiceID = 2, VehicleID = 2, Description = " The windows were not closing ",ServiceDate = new DateTime(2023, 1, 1) }
            );

            modelBuilder.Entity<ServiceHistory>().HasData(
                new ServiceHistory { Id = 1, VehicleID = 1, ServiceDate = new DateTime(2022, 1, 1) },
                new ServiceHistory { Id = 2, VehicleID = 2, ServiceDate = new DateTime(2023, 1, 1) }
            );

            modelBuilder.Entity<VehicleChecklist>(entity =>
            {
                entity.OwnsOne(v => v.ExteriorChecks);
                entity.OwnsOne(v => v.InteriorChecks);
                entity.OwnsOne(v => v.UnderTheHoodChecks);
                entity.OwnsOne(v => v.FunctionalTests);
                entity.OwnsOne(v => v.SafetyEquipment);
                entity.OwnsOne(v => v.Documentation);
            });
                
        }

    }

}

