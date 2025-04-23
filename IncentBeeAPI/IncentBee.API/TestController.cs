using Microsoft.AspNetCore.Mvc;
using System;

namespace IncentBee.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IncentBeeDbContext _context;
        
        public TestController(IncentBeeDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GetDbStatus()
        {
            try
            {
                bool canConnect = _context.Database.CanConnect();
                return Ok(new { 
                    DatabaseConnection = canConnect ? "Success" : "Failed",
                    Message = "API is working correctly",
                    ServerTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}