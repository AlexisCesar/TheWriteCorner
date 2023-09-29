using AuthAPI.Data.DTOs;
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

        public UsersController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CadastraUsuario
             (CreateUserDto dto)
        {
            await _userService.CadastraUsuario(dto);
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