using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class UserDbContext : IdentityDbContext<User, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>

    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        { }

        public DbSet<Driver> Drivers { get; set; } // DbSet for Driver entity
        public DbSet<Admin> Admins { get; set; } // DbSet for Admin entity
        public override DbSet<User> Users { get; set; } //DbSet for User entity

        public DbSet<Project> Projects { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; } //DbSet for Audit log entity

        public DbSet<Feedback> Feedbacks { get; set; } //DbSet for Feedback entity


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure User Entity with Asp Identity Standards

            //Mapping
            // Configure the ApplicationUser to use the 'Users' table
            /*modelBuilder.Entity<User>()
                .ToTable("Users");

            // Add configuration for the Driver entity if needed
            modelBuilder.Entity<Driver>()
                .ToTable("Users");
*/
            // Configure the User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // Ensure Id is the primary key
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Surname).HasMaxLength(50);
            });
            modelBuilder.Entity<User>()
        .HasDiscriminator<string>("Role")
        .HasValue<Admin>("Admin")
        .HasValue<User>("User")
        .HasValue<Driver>("Driver");

            //Audit Log Config

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Details).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
            });

            //Feedback config

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.UserName).IsRequired().HasMaxLength(256);
                entity.Property(f => f.Email).IsRequired().HasMaxLength(256);
                entity.Property(f => f.Message).IsRequired();
                entity.Property(f => f.Timestamp).IsRequired();
                entity.Property(f => f.Rating).IsRequired().HasDefaultValue(0); 
            });

            //Config of IdentyUserRole relationship
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                //customizations for IdentityRole entity
                //customizations for IdentityRole entity
            });


            //My Additions conlusion


            /*// Map the Driver entity to the Users table
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
            );*/
        }
    }
}
