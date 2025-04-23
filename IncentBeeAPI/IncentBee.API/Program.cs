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
            
            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            // Serve static files from wwwroot
            app.UseStaticFiles();
            
            // Database connection check
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
                try
                {
                    dbContext.Database.CanConnect();
                    Console.WriteLine("Database connection successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                }
            }

            app.UseRouting();
            app.UseAuthorization();
            
            app.MapControllers();
            
            // Add a route to the callbacks viewer HTML
            app.MapGet("/callbacks", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("/var/www/incentbee/CallbacksViewer.html");
            });
            
            app.Run();
        }
    }
}