using Microsoft.EntityFrameworkCore;

public class WeatherForecastDbContext : DbContext
{
    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
        : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Explicitly defining primary key using Fluent API (optional, as primary key is already defined in the entity)
        modelBuilder.Entity<WeatherForecast>()
            .HasKey(wf => wf.Id); // Set 'Id' as the primary key
    }
}
