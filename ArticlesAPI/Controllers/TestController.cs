using ArticlesAPI.RabbitMq;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IArticlesService _articlesService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public TestController(IArticlesService service, IRabbitMqPublisher rabbitMqPublisher)
        {
            _articlesService = service;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpGet(Name = "GetValueFromDatabase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> Get()
        {
            var result = await _articlesService.GetAsync();

            return Ok(result);
        }
        
        [HttpPost("broker", Name = "PublishMessageOnBroker")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult PublishMessage(string message)
        {
            try
            {
                _rabbitMqPublisher.PublishMessage("banana", message);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }
    }
}
