using System.Text.Json;
using AuthAPI.Data.DTOs;
using AuthAPI.RabbitMq;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly string Exchange = "usersOperations";

        public UsersController(ILogger<UsersController> logger, UserService userService, IRabbitMqPublisher rabbitMqPublisher)
        {
            _logger = logger;
            _userService = userService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CadastraUsuario
             (CreateUserDto dto)
        {
            await _userService.CadastraUsuario(dto);
            _rabbitMqPublisher.PublishMessage(Exchange, dto.EmailAddress, "email.registered");

            return Ok("User registered!");

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDto dto)
        {
            var token = await _userService.Login(dto);
            return Ok(token);
        }
    }
}