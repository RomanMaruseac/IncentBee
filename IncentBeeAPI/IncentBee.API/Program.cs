using Microsoft.EntityFrameworkCore;
using IncentBee.API;
using IncentBee.API.Models;

namespace IncentBee.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register services
            builder.Services.AddDbContext<IncentBeeDbContext>(options =>
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
            
            // Database connection check
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IncentBeeDbContext>();
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
            // Add a route to the admin panel HTML
            app.MapGet("/admin", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("/var/www/incentbee/AdminPanel.html");
            });
            
            app.Run();
        }
    }
}