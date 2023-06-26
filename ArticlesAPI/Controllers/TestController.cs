using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet(Name = "GetValueFromDatabase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Get()
        {
            return Ok("Value from database: ...");
        }
    }
}
