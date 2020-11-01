using Microsoft.EntityFrameworkCore;
using RoutingAssistant.Core.Entities;

namespace RoutingAssistant.DataLayer.Implementations
{
    public class RoutingAssistantDbContext : DbContext
    {
        public RoutingAssistantDbContext(DbContextOptions<RoutingAssistantDbContext> options)
           : base(options)
        {
        }

        public DbSet<TourEntity> Tours { get; set; }
        public DbSet<StopEntity> Stops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TourEntity>()
                .HasMany(t => t.Stops)
                .WithOne(s => s.Tour)
                .HasForeignKey(s => s.TourId);
        }
    }
}
