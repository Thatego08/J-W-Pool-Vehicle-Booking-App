using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ProjectRate> ProjectRates { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Rate> Rates { get; set; }
        public DbSet<RateType> RateTypes { get; set; }
        public DbSet<FAQ> Faqs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);

            // Many-to-many configuration for ProjectRate
            builder.Entity<ProjectRate>()
                .HasKey(pr => new { pr.ProjectID, pr.RateID });

            builder.Entity<ProjectRate>()
                .HasOne(pr => pr.Project)
                .WithMany(p => p.ProjectRates)
                .HasForeignKey(pr => pr.ProjectID);

            builder.Entity<ProjectRate>()
                .HasOne(pr => pr.Rate)
                .WithMany(r => r.ProjectRates)
                .HasForeignKey(pr => pr.RateID);

            // Relationship between Rate and RateType
            builder.Entity<Rate>()
                .HasOne(r => r.RateType)
                .WithMany(rt => rt.Rates)
                .HasForeignKey(r => r.RateTypeID);

            // Seeding RateTypes
            builder.Entity<RateType>().HasData(
                new RateType { RateTypeID = 1, RateTypeName = "Half-Day Rate" },
                new RateType { RateTypeID = 2, RateTypeName = "Full-Day Rate" },
                new RateType { RateTypeID = 3, RateTypeName = "Travel Kilometer Rate" },
                new RateType { RateTypeID = 4, RateTypeName = "Special Rate" }
            );

            // Example seeding for Rate and ProjectRate relationship
            builder.Entity<Rate>().HasData(
                new Rate { RateID = 1, RateTypeID = 1, RateValue = 100, ApplicableTimePeriod = "7am-5pm", Conditions = "Standard" },
                new Rate { RateID = 2, RateTypeID = 2, RateValue = 200, ApplicableTimePeriod = "6am-5pm", Conditions = "Standard" }
            );

            builder.Entity<ProjectRate>().HasData(
                new ProjectRate { ProjectID = 1, RateID = 1 },
                new ProjectRate { ProjectID = 2, RateID = 2 }
            );
        }


    }
}




