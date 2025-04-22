using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IncentBee.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastDbContext _context;

        public WeatherForecastController(WeatherForecastDbContext context)
        {
            _context = context;
        }

        // GET: api/WeatherForecast
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecasts()
        {
            return await _context.WeatherForecasts.ToListAsync();
        }

        // GET: api/WeatherForecast/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecast>> GetWeatherForecast(int id)
        {
            var weatherForecast = await _context.WeatherForecasts.FindAsync(id);

            if (weatherForecast == null)
            {
                return NotFound();
            }

            return weatherForecast;
        }

        // POST: api/WeatherForecast
        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> PostWeatherForecast(WeatherForecast weatherForecast)
        {
            _context.WeatherForecasts.Add(weatherForecast);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeatherForecast), new { id = weatherForecast.Id }, weatherForecast);
        }
    }
}
