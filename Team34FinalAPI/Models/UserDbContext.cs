using Microsoft.EntityFrameworkCore;
namespace Team34FinalAPI.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) 
        { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<User>().ToTable("Users");

            //Seed data

            modelBuilder.Entity<User>().HasData(

                  new Driver
                  {
                     UserName = "NtsaMa",
                     Name = "Ntsako",
                     Surname = "Malepfana",
                     Email = "NtsakoMalepfan@gmail.com",
                     PhoneNumber = 0715341011,
                     Password = "0000"

                  },

                    new Driver
                    {
                        UserName = "MahlSa",
                        Name = "Sabelo",
                        Surname = "Mahlangu",
                        Email = "mahlanguSabelo@gmail.com",
                        PhoneNumber = 0713341011,
                        Password = "0000"

                    }
                );

          
        }
    } 
}
