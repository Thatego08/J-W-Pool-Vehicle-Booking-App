using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class TripDbContext : DbContext
    {
        public TripDbContext(DbContextOptions<TripDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripMedia> TripMedia { get; set; }
        public DbSet<RefuelVehicle> RefuelVehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().ToTable("Trips");
            modelBuilder.Entity<TripMedia>().ToTable("TripMedia");
            modelBuilder.Entity<RefuelVehicle>().ToTable("RefuelVehicles");
            // Configure the relationship between Trip and TripMedia
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.TripMedia)
                .WithOne(m => m.Trip)
                .HasForeignKey(m => m.TripId);


            // Configure the relationship between Trip and RefuelVehicle
            modelBuilder.Entity<Trip>()
         .HasMany(t => t.RefuelVehicles)
         .WithOne(rv => rv.Trip)
         .HasForeignKey(rv => rv.TripId);
            // This will make TripId optional


            // Configure the relationship between Trip and Vehicle
            modelBuilder.Entity<Trip>()
                .HasOne<Vehicle>(t => t.Vehicle)
                .WithMany() // Assuming a vehicle can have multiple trips
                .HasForeignKey(t => t.VehicleId)
                .IsRequired();

            // Ignore the Vehicle entity to avoid creating its table
            modelBuilder.Ignore<Vehicle>();

            modelBuilder.Entity<RefuelVehicle>()
        .Property(rv => rv.FuelQuantity)
        .HasPrecision(18, 2); // Adjust precision and scale as needed

            modelBuilder.Entity<Trip>()
                .Property(t => t.FuelAmount)
                .HasPrecision(18, 2); // Adjust precision and scale as needed
        }
    }
}