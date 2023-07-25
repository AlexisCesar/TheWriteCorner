using ArticlesAPI.DTOs.Command;
using ArticlesAPI.Models;
using ArticlesAPI.RabbitMq;
using ArticlesAPI.Services;
using AutoMapper;
using FluentValidation;
using log4net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Text.Json;

namespace ArticlesAPI.Controllers
{
    [ApiController]
    [Route("api/v1/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IArticlesService _articlesService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly String exchange = "articlesOperations";

        private readonly IValidator<Article> _articleValidator;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(ArticlesController));

        public ArticlesController(
            IArticlesService service,
            IRabbitMqPublisher rabbitMqPublisher,
            IMapper mapper,
            IValidator<Article> articleValidator
        )
        {
            _articlesService = service;
            _rabbitMqPublisher = rabbitMqPublisher;
            _mapper = mapper;
            _articleValidator = articleValidator;
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
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
            }
            
            return result.Count() == 0 ? NoContent() : Ok(result);         
        }

        [HttpGet]
        [Route(template: "{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticleById([FromRoute] string id)
        {
            if (ObjectId.TryParse(id, out _) && !string.IsNullOrEmpty(id))
            {
                Article? result;
                try
                {
                    result = await _articlesService.GetAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
                }

                return result == null ? NotFound("We could not find this article in our database") : Ok(result) ;
            }

            return BadRequest("Invalid ID");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleCommand article)
        {
            if (article == null) return BadRequest();            

            try
            {
                var newArticle = _mapper.Map<Article>(article);

                var validationResult = _articleValidator.Validate(newArticle);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
                } 

                await _articlesService.CreateAsync(newArticle);

                _rabbitMqPublisher.PublishMessage(exchange, JsonSerializer.Serialize(newArticle));

                return Created($"api/v1/articles/{newArticle.Id}", newArticle);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
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
            if (ObjectId.TryParse(id, out _) && !string.IsNullOrEmpty(id))
            {
                try
                {
                    await _articlesService.RemoveAsync(id);

                    _rabbitMqPublisher.PublishMessage(exchange, JsonSerializer.Serialize(new Article() { Id = id}));

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
                }
            }

            return BadRequest("Invalid ID");
        }

        [HttpPut]
        [Route(template: "{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticle([FromRoute] string id, [FromBody] UpdateArticleCommand article)
        {
            if (ObjectId.TryParse(id, out _) && !string.IsNullOrEmpty(id))
            {
                try
                {
                    var articleInDatabase = await _articlesService.GetAsync(id);

                    if (articleInDatabase == null) return NotFound("An article with this ID was not found.");

                    var articleToUpdate = _mapper.Map<Article>(article);

                    var validationResult = _articleValidator.Validate(articleToUpdate);

                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
                    }

                    await _articlesService.UpdateAsync(id, articleToUpdate);

                    _rabbitMqPublisher.PublishMessage(exchange, JsonSerializer.Serialize(article));

                    return Ok(article);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with our database, please try again later.");
                }
            }

            return BadRequest("Invalid ID");
        }
    }
}