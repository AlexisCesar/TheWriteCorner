using ArticlesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IArticlesService _articlesService;

        public TestController(IArticlesService service)
        {
            _articlesService = service;
        }

        [HttpGet(Name = "GetValueFromDatabase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> Get()
        {
            var result = await _articlesService.GetAsync();

            return Ok(result);
        }
    }
}
