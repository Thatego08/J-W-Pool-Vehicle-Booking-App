using Microsoft.EntityFrameworkCore;
using System;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasData(
                new Project { ProjectID = 1, ProjectName = "Construction Project", JobNo = 1001, TaskCode = 2001, ActivityCode = 3001 },
                new Project { ProjectID = 2, ProjectName = "Mining Project", JobNo = 1002, TaskCode = 2002, ActivityCode = 3002 },
                new Project { ProjectID = 3, ProjectName = "Bridge Building Project", JobNo = 1003, TaskCode = 2003, ActivityCode = 3003 },
                new Project { ProjectID = 4, ProjectName = "City Infrastructure Project", JobNo = 1004, TaskCode = 2004, ActivityCode = 3004 },
                new Project { ProjectID = 5, ProjectName = "Impact Analysis Project", JobNo = 1005, TaskCode = 2005, ActivityCode = 3005 }
            );

        }
    }

    
}
