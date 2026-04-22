using GymAppC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymAppC.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Workout> Workouts => Set<Workout>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()

                .HasMany(u => u.Workouts)

                .WithOne(w => w.User)

                .HasForeignKey(w => w.UserId)

                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()

                .HasMany(w => w.Exercises)

                .WithOne(e => e.Workout)

                .HasForeignKey(e => e.WorkoutId)

                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
