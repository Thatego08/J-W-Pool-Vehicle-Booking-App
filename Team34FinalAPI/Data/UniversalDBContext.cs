using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Data
{
    public class UniversalDBContext: DbContext
    {
        public UniversalDBContext(DbContextOptions<UniversalDBContext> options) : base(options) { }

        //AppDbContext Configs : Rates, RateTypes

        public DbSet<Rate> Rates { get; set; }
        public DbSet<RateType> RateTypes { get; set; }

        //BookingDbContext Configs : Bookings, Vehicles, Projects, Events, Inspection Lists

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<InspectionList> InspectionLists
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //AppDb Relationships

            // Configure relationships
            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Project)
                .WithMany(p => p.Rates)
                .HasForeignKey(r => r.ProjectID);

        }

    }
}
