using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class RateEEDBContext :DbContext
    {

        public RateEEDBContext(DbContextOptions<RateEEDBContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RatesEE> RatesEE { get; set; }   // Add this

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many: Project -> RatesEE
            modelBuilder.Entity<RatesEE>()
                .HasOne(r => r.Project)
                .WithMany(p => p.RatesEE)  // You'll need to add a collection nav property in Project
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // or Restrict, depending on your business rule

            // Additional configurations (e.g., indexes)
            modelBuilder.Entity<RatesEE>()
                .HasIndex(r => new { r.ProjectId, r.IsActive });
        }
    }
}
