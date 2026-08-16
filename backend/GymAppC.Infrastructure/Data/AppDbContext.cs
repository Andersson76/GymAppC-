using GymAppC.Domain.Constants;
using GymAppC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymAppC.Infrastructure.Data;

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(user => user.Name)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(user => user.Email)
                .HasMaxLength(256)
                .IsRequired();
            entity.Property(user => user.Role)
                .HasMaxLength(32)
                .HasDefaultValue(AppRoles.User)
                .IsRequired();
            entity.HasIndex(user => user.Email)
                .IsUnique();

            entity.HasMany(user => user.Workouts)
                .WithOne(workout => workout.User)
                .HasForeignKey(workout => workout.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Workout>()
            .HasMany(workout => workout.Exercises)
            .WithOne(exercise => exercise.Workout)
            .HasForeignKey(exercise => exercise.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
