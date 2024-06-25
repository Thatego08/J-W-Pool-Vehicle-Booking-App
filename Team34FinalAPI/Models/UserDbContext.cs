using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) 
        { }

        public DbSet<Driver> Drivers { get; set; } // DbSet for Driver entity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            // Map the Driver entity to the Users table
            modelBuilder.Entity<Driver>().ToTable("Users");

            // Seed data for Drivers (not Users)
            modelBuilder.Entity<Driver>().HasData(
                new Driver
                {
                    UserName = "NtsaMa",
                    Name = "Ntsako",
                    Surname = "Malepfana",
                    Email = "NtsakoMalepfan@gmail.com",
                    PhoneNumber = 715341011, // Remove leading zero for integers
                    Password = "0000"
                },
                new Driver
                {
                    UserName = "MahlSa",
                    Name = "Sabelo",
                    Surname = "Mahlangu",
                    Email = "mahlanguSabelo@gmail.com",
                    PhoneNumber = 713341011, // Remove leading zero for integers
                    Password = "0000"
                }
            );
        }
    }
}
