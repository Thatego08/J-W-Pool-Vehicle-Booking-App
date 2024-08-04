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

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.RateType)
                .WithMany(rt => rt.Rates)
                .HasForeignKey(r => r.RateTypeID);

            modelBuilder.Entity<RateType>().HasData(
               new RateType { RateTypeID = 1, RateTypeName = "half-day rate" },
               new RateType { RateTypeID = 2, RateTypeName = "full-day rate" },
               new RateType { RateTypeID = 3, RateTypeName = "kilometer rate" }

               );

            modelBuilder.Entity<Rate>().HasData(
          new Rate { RateID = 1, ProjectID = 1, RateTypeID = 1, RateValue = 100, ApplicableTimePeriod = "6am-12pm" },
          new Rate { RateID = 2, ProjectID = 2, RateTypeID = 2, RateValue = 200, ApplicableTimePeriod = "12pm-6pm" },
          new Rate { RateID = 3, ProjectID = 3, RateTypeID = 2, RateValue = 200, ApplicableTimePeriod = null }


      );
        }

    }
}
