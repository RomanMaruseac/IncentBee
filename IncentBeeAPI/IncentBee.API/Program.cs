var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with PostgreSQL connection string
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

// Sample data generation and API endpoint
app.MapGet("/weatherforecast", async (WeatherForecastDbContext dbContext) =>
{
    return await dbContext.WeatherForecasts.ToListAsync();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
