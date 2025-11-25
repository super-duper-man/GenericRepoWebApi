using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GenericRepoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController(HealthCheckService healthCheckService) : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService = healthCheckService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HealthReport report = await _healthCheckService.CheckHealthAsync();
            return Ok(report.Status.ToString());
        }
    }
}
