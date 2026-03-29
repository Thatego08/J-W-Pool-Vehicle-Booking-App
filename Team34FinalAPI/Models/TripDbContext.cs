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
        public DbSet<PostCheck> PostChecks { get; set; }
        public DbSet<Project> Projects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().ToTable("Trips");
            modelBuilder.Entity<TripMedia>().ToTable("TripMedia");
            modelBuilder.Entity<RefuelVehicle>().ToTable("RefuelVehicles");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<PreChecklist>().ToTable("PreChecklists");
            modelBuilder.Entity<PostCheck>().ToTable("PostCheck");

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BookingID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostCheck>()
   .HasOne(p => p.Trip)
   .WithMany(t => t.PostChecks)
   .HasForeignKey(p => p.TripId)
   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostCheck>()
                .HasMany(p => p.TripMedia)
                .WithOne(t => t.PostCheck)
                .HasForeignKey(t => t.PostCheckId);





            modelBuilder.Entity<PreChecklist>()
       .HasOne(pc => pc.Booking)
       .WithMany(b => b.PreChecklists) // 'PreChecklists' navigation property in Booking
       .HasForeignKey(pc => pc.BookingID)
       .OnDelete(DeleteBehavior.Cascade); // Delete PreChecklists if Booking is deleted (optional)



            modelBuilder.Entity<Trip>()
                .HasOne(t => t.PreChecklist)
                .WithOne()
                .HasForeignKey<Trip>(t => t.PreChecklistId)
                .OnDelete(DeleteBehavior.Restrict);

            // Adding the foreign key for BookingID in PreChecklist




            modelBuilder.Entity<Trip>()
                .HasMany(t => t.RefuelVehicles)
                .WithOne(rv => rv.Trip)
                .HasForeignKey(rv => rv.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Ignore<Vehicle>();

            modelBuilder.Entity<RefuelVehicle>()
                .Property(rv => rv.FuelQuantity)
                .HasPrecision(18, 2);
        }
    }
}