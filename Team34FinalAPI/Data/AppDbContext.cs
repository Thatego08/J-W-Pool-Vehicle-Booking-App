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
               new RateType { RateTypeID = 1, RateTypeName = "half-day rate" },
               new RateType { RateTypeID = 2, RateTypeName = "full-day rate" },
               new RateType { RateTypeID = 3, RateTypeName = "kilometer rate" }
           );

        }


    }
}




