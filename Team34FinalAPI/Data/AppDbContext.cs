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

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Rate>()
                .HasOne(r => r.Project)
                .WithMany(p => p.Rates)
                .HasForeignKey(r => r.ProjectID);

            builder.Entity<Rate>()
                .HasOne(r => r.RateType)
                .WithMany(rt => rt.Rates)
                .HasForeignKey(r => r.RateTypeID);

            builder.Entity<RateType>().HasData(
               new RateType { RateTypeID = 1, RateTypeName = "Half-Day Rate" },
               new RateType { RateTypeID = 2, RateTypeName = "Full-Day Rate" },
               new RateType { RateTypeID = 3, RateTypeName = "Travel Kilometer Rate" },
               new RateType { RateTypeID = 4, RateTypeName = "Special Rate" }

               );

            builder.Entity<Rate>().HasData(
          new Rate { RateID = 1, ProjectID = 1, RateTypeID = 1, RateValue = 100, ApplicableTimePeriod = "7am-5pm", Conditions = "Standard" },
          new Rate { RateID = 2, ProjectID = 2, RateTypeID = 2, RateValue = 200, ApplicableTimePeriod = "6am-5pm", Conditions = "Standard" },
          new Rate { RateID = 3, ProjectID = 3, RateTypeID = 2, RateValue = 200, ApplicableTimePeriod = "6am-5pm", Conditions = "Standard" }


      );

        }


    }
}




