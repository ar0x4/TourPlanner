namespace TourPlanner.DAL;

using Microsoft.EntityFrameworkCore;
using TourPlanner.Models;

public class TourPlannerDbContext : DbContext
{
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<TourLog> TourLogs => Set<TourLog>();
    public DbSet<User> Users => Set<User>();

    public TourPlannerDbContext(DbContextOptions<TourPlannerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(256);
            entity.Property(t => t.From).IsRequired().HasMaxLength(256);
            entity.Property(t => t.To).IsRequired().HasMaxLength(256);
            entity.Property(t => t.TransportType).IsRequired().HasMaxLength(64);
            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TourLog>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasOne<Tour>()
                  .WithMany()
                  .HasForeignKey(l => l.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
