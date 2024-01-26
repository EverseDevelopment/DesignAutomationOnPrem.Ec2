using Microsoft.AspNetCore.Mvc;

namespace DesignAutomationEc2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Sample Data");
        }
    }
}


