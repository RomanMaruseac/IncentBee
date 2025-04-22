using Microsoft.EntityFrameworkCore;
using IncentBee;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Test DB connection endpoint
app.MapGet("/testdb", async (WeatherForecastDbContext db) =>
{
    try
    {
        var forecast = await db.WeatherForecasts.ToListAsync();
        return forecast.Any() 
            ? Results.Ok("Database connection is working. Data fetched.")
            : Results.NotFound("Database connected, but no data found.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error connecting to database: {ex.Message}");
    }
});

app.Run();
