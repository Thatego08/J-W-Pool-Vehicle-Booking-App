using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<InspectionList> InspectionLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly mapping tables
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<InspectionList>().ToTable("InspectionLists");

            // Configure relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Project)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for InspectionList
            modelBuilder.Entity<InspectionList>().HasData(
                new InspectionList { ChecklistID = 1, Item = "Windscreen", IsCompleted = false },
                new InspectionList { ChecklistID = 2, Item = "TyreCondition", IsCompleted = false },
                new InspectionList { ChecklistID = 3, Item = "FirstAidKit", IsCompleted = false },
                new InspectionList { ChecklistID = 4, Item = "Brakes", IsCompleted = false },
                new InspectionList { ChecklistID = 5, Item = "Mirrors", IsCompleted = false },
                new InspectionList { ChecklistID = 6, Item = "Lights", IsCompleted = false }
            );
        }
    }
}
