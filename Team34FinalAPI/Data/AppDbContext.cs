using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Rate> Rates { get; set; }
        public DbSet<RateType> RateTypes { get; set; }
        public DbSet<FAQ> Faqs { get; set; }

        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Rate>()
                .HasOne(r => r.Project)
                .WithMany(p => p.Rates)
                .HasForeignKey(r => r.ProjectID);


            builder.Entity<Rate>().HasData(
          new Rate { RateID = 1, ProjectID = 1,  RateValue = 100, ApplicableTimePeriod = "7am-5pm", Conditions = "Half-Day Rate" },
          new Rate { RateID = 2, ProjectID = 2,  RateValue = 200, ApplicableTimePeriod = "6am-5pm", Conditions = "Full-Day Rate" },
          new Rate { RateID = 3, ProjectID = 3, RateValue = 200, ApplicableTimePeriod = "2pm-4pm", Conditions = "Travel Kilometer Rate" }


      );

        }


    }
}




