using Microsoft.AspNetCore.Mvc;

namespace WebApiBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new { 
                Status = "Alive", 
                Timestamp = DateTime.UtcNow,
                Message = "API is running"
            });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { 
                Status = "Healthy", 
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            });
        }
    }
}