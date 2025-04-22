using Microsoft.EntityFrameworkCore;
using IncentBee.API;

namespace IncentBee.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register services
            builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            var app = builder.Build();

            // Add a simple database check
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
                try
                {
                    dbContext.Database.CanConnect(); // This checks if the connection to the DB is working
                    Console.WriteLine("Database connection successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                }
            }

            app.MapControllers();
            app.Run();
        }
    }
}
