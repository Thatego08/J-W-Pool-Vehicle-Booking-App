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
        public DbSet<Vehicle> Vehicles { get; set; } // Add this line

        public DbSet<Project> Projects { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<InspectionList> InspectionLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Explicitly mapping tables
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");

            // Configure the relationship between Project and Booking
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Bookings)
                .WithOne(b => b.Project)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.Restrict if you want to prevent deletion

            // Configure the relationship between Project and Rate
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Rates)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectID)
                .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.Restrict if you want to prevent deletion

            
                //Inspection List stuff

                // Seed data for the InspectionChecklist
                modelBuilder.Entity<InspectionList>().HasData(
                new InspectionList { ChecklistID = 1, Item = "Windscreen", IsCompleted = false },
                new InspectionList { ChecklistID = 2, Item = "Tires", IsCompleted = false },
                new InspectionList { ChecklistID = 3, Item = "Lights", IsCompleted = false },
                new InspectionList { ChecklistID = 4, Item = "Brakes", IsCompleted = false },
                new InspectionList { ChecklistID = 5, Item = "First Aid Kit", IsCompleted = false },
                new InspectionList { ChecklistID = 6, Item = "Tool Kit", IsCompleted = false }
            );

            //End of my adjustments


            //Project config 
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.ProjectID);
                entity.Property(p => p.ProjectNumber).IsRequired().HasMaxLength(100);
                entity.Property(p => p.JobNo).IsRequired();
                entity.Property(p => p.Description).IsRequired();
                entity.Property(p => p.TaskCode).IsRequired();
                entity.Property(p => p.ActivityCode).IsRequired();

            });


            modelBuilder.Entity<Project>().HasData(

            new Project { ProjectID = 1, ProjectNumber = 120, Description = "Mpumalanga Star Project", JobNo = 1001, TaskCode = 2001, ActivityCode = 3001,  },
            new Project { ProjectID = 2, ProjectNumber = 128, Description = "Mpumalanga Star Project", JobNo = 1002, TaskCode = 2002, ActivityCode = 3002,  },
            new Project { ProjectID = 3, ProjectNumber = 129, Description = "Mpumalanga Star Project", JobNo = 1003, TaskCode = 2003, ActivityCode = 3003,  },
            new Project { ProjectID = 4, ProjectNumber = 130, Description = "Mpumalanga Star Project", JobNo = 1004, TaskCode = 2004, ActivityCode = 3004,  },
            new Project { ProjectID = 5, ProjectNumber = 131, Description = "Mpumalanga Star Project", JobNo = 1005, TaskCode = 2005, ActivityCode = 3500, }

        );
            





        }
    }

}
