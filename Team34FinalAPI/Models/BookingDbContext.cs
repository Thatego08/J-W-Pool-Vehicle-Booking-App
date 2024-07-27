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
            modelBuilder.Entity<Project>().Property(p => p.HalfDayRate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Project>().Property(p => p.FullDayRate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
            modelBuilder.Entity<Booking>().Property(b => b.RateType).IsRequired();

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

            // RateType seed data
            modelBuilder.Entity<RateType>().HasData(
                new RateType { RateTypeID = 1, TypeName = "HalfDay" },
                new RateType { RateTypeID = 2, TypeName = "FullDay" },
                new RateType { RateTypeID = 3, TypeName = "Kilometer" }
                );
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


            //Rate config
            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasKey(r => r.RateId);
                entity.Property(r => r.RateType).IsRequired();
                entity.Property(r => r.RateValue).HasColumnType("decimal(18,2)"); // Ensure column type is set
                entity.HasOne(r => r.Project)
                      .WithMany(p => p.Rates) // Assuming a Rate belongs to a Project
                      .HasForeignKey(r => r.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Project config 
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.ProjectID);
                entity.Property(p => p.ProjectNumber).IsRequired().HasMaxLength(100);
                entity.Property(p => p.JobNo).IsRequired();
                entity.Property(p => p.TaskCode).IsRequired();
                entity.Property(p => p.ActivityCode).IsRequired();

            });


            modelBuilder.Entity<Project>().HasData(


            new Project { ProjectID = 1, ProjectNumber = 120, JobNo = 1001, TaskCode = 2001, ActivityCode = 3001,  },
            new Project { ProjectID = 2, ProjectNumber = 128, JobNo = 1002, TaskCode = 2002, ActivityCode = 3002,  },
            new Project { ProjectID = 3, ProjectNumber = 129, JobNo = 1003, TaskCode = 2003, ActivityCode = 3003,  },
            new Project { ProjectID = 4, ProjectNumber = 130, JobNo = 1004, TaskCode = 2004, ActivityCode = 3004,  },
            new Project { ProjectID = 5, ProjectNumber = 131, JobNo = 1005, TaskCode = 2005, ActivityCode = 3005,  }

        );
            





        }
    }

}
