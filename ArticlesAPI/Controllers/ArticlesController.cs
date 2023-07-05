using ArticlesAPI.Models;
using ArticlesAPI.RabbitMq;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    [ApiController]
    [Route("api/v1/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService _articlesService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public ArticlesController(IArticlesService service, IRabbitMqPublisher rabbitMqPublisher)
        {
            _articlesService = service;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Article))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllArticlesAsync()
        {
            List<Article> result;
            try
            {
                result = await _articlesService.GetAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
            }
            
            return result.Count() == 0 ? NoContent() : Ok(result);         
        }

        [HttpGet]
        [Route(template: "{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArticleById([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Article? result;
            try
            {
                result = await _articlesService.GetAsync(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
            }        

            return result == null ? NotFound("We could not find this article in our database") : Ok(result) ;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateArticle([FromBody] Article article)
        {
            if (article == null) return BadRequest();

            try
            {
                await _articlesService.CreateAsync(article);
                return Created($"api/v1/articles/{article.Id}", article);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
            }
        }

        [HttpDelete]
        [Route(template: "{id}")]  
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteArticle([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            try
            {
                await _articlesService.RemoveAsync(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
            }
        }

        [HttpPut]
        [Route(template: "{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticle([FromRoute] string id, [FromBody] Article article)
        {
            if (string.IsNullOrEmpty(id) || article == null) return BadRequest();

            try
            {
                await _articlesService.UpdateAsync(id, article);
                return Ok(article);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
            }
        }
    }
}