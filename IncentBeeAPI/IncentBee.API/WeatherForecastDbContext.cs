using Microsoft.EntityFrameworkCore;

namespace IncentBee.API
{
    public class WeatherForecastDbContext : DbContext
    {
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure WeatherForecast entity
            modelBuilder.Entity<WeatherForecast>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Summary).HasMaxLength(100);
            });
            
            // Add seed data if needed
            modelBuilder.Entity<WeatherForecast>().HasData(
                new WeatherForecast { Id = 1, Date = DateOnly.FromDateTime(DateTime.Now), TemperatureC = 25, Summary = "Warm" },
                new WeatherForecast { Id = 2, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 30, Summary = "Hot" }
            );
        }
    }
}