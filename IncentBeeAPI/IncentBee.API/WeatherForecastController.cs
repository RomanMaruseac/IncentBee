using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IncentBee.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(WeatherForecastDbContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/WeatherForecast
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecasts()
        {
            try
            {
                return await _context.WeatherForecasts.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weather forecasts");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        // GET: api/WeatherForecast/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecast>> GetWeatherForecast(int id)
        {
            try
            {
                var weatherForecast = await _context.WeatherForecasts.FindAsync(id);

                if (weatherForecast == null)
                {
                    return NotFound();
                }

                return weatherForecast;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting weather forecast with id {id}");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        // POST: api/WeatherForecast
        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> PostWeatherForecast(WeatherForecast weatherForecast)
        {
            try
            {
                _context.WeatherForecasts.Add(weatherForecast);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetWeatherForecast), new { id = weatherForecast.Id }, weatherForecast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating weather forecast");
                return StatusCode(500, "Internal server error occurred");
            }
        }
        
        // PUT: api/WeatherForecast/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeatherForecast(int id, WeatherForecast weatherForecast)
        {
            if (id != weatherForecast.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(weatherForecast).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherForecastExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating weather forecast with id {id}");
                return StatusCode(500, "Internal server error occurred");
            }

            return NoContent();
        }

        // DELETE: api/WeatherForecast/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherForecast(int id)
        {
            try
            {
                var weatherForecast = await _context.WeatherForecasts.FindAsync(id);
                if (weatherForecast == null)
                {
                    return NotFound();
                }

                _context.WeatherForecasts.Remove(weatherForecast);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting weather forecast with id {id}");
                return StatusCode(500, "Internal server error occurred");
            }
        }

        private bool WeatherForecastExists(int id)
        {
            return _context.WeatherForecasts.Any(e => e.Id == id);
        }
    }
}