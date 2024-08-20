using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class TripDbContext : DbContext
    {
        public TripDbContext(DbContextOptions<TripDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripMedia> TripMedia { get; set; }
        public DbSet<RefuelVehicle> RefuelVehicles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PreChecklist> PreChecklists { get; set; }

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

            modelBuilder.Entity<Trip>()
          .HasOne(t => t.Booking)        // Trip has one Booking
          .WithMany(b => b.Trips)        // Booking has many Trips
          .HasForeignKey(t => t.BookingID) // Foreign key in Trip table
          .OnDelete(DeleteBehavior.Restrict); // Set Restrict to avoid cascading delete

            // Other entity configurations
            modelBuilder.Entity<PreChecklist>()
       .HasOne<Trip>() // Configure the relationship
       .WithOne() // One-to-one relationship
       .HasForeignKey<PreChecklist>(pc => pc.TripId); // Foreign key in PreChecklist

            modelBuilder.Entity<Trip>()
               .HasMany(t => t.TripMedia)
               .WithOne(tm => tm.Trip)
               .HasForeignKey(tm => tm.TripId);

            //modelBuilder.Entity<PreChecklist>()
       //.HasMany(p => p.TripMedia)
       //.WithOne(m => m.PreChecklist)
       //.HasForeignKey(m => m.Id)
      // .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between Trip and RefuelVehicle
            modelBuilder.Entity<Trip>()
         .HasMany(t => t.RefuelVehicles)
         .WithOne(rv => rv.Trip)
         .HasForeignKey(rv => rv.TripId);
            // This will make TripId optional

            modelBuilder.Entity<RefuelVehicle>()
       .HasOne(rv => rv.Trip)
       .WithMany(t => t.RefuelVehicles)
       .HasForeignKey(rv => rv.TripId)
       .OnDelete(DeleteBehavior.ClientSetNull); // Allow null values
                                                // Or use DeleteBehavior.ClientSetNull if you want null values allowed


            // Configure the relationship between Trip and Vehicle
            //modelBuilder.Entity<Trip>()
            //  .HasOne<Vehicle>(t => t.Vehicle)
            // .WithMany() // Assuming a vehicle can have multiple trips
            // .HasForeignKey(t => t.VehicleId)
            // .IsRequired();

            // Ignore the Vehicle entity to avoid creating its table
            modelBuilder.Ignore<Vehicle>();

            modelBuilder.Entity<RefuelVehicle>()
        .Property(rv => rv.FuelQuantity)
        .HasPrecision(18, 2); // Adjust precision and scale as needed

            //modelBuilder.Entity<Trip>()
                //.Property(t => t.FuelAmount)
                //.HasPrecision(18, 2); // Adjust precision and scale as needed
        }
    }
}