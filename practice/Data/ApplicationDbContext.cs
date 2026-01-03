using Microsoft.EntityFrameworkCore;
using practice.Models;

namespace practice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                   .HasIndex(u => u.VoterIdNumber)
                   .IsUnique();


            // Candidate configuration
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.User)
                .WithOne(u => u.CandidateProfile)
                .HasForeignKey<Candidate>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vote configuration
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Election)
                .WithMany(e => e.Votes)
                .HasForeignKey(v => v.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Voter)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.VoterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Candidate)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure a voter can only vote once per election
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.ElectionId, v.VoterId })
                .IsUnique();

            // Seed default admin user (password: Admin@123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "System Administrator",
                    Email = "admin@votingsystem.com",
                    PhoneNumber = "9999999999",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsVerified = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed default election
            modelBuilder.Entity<Election>().HasData(
                new Election
                {
                    Id = 1,
                    Title = "General Election 2024",
                    Description = "National General Election for selecting representatives",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    ElectionType = "General",
                    IsActive = true,
                    ResultsPublished = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
