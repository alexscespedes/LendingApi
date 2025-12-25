using LendingApi.Application.Services.Auth;
using LendingApi.Application.Services.Auth.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IAuthService service, JwtService jwtService, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto requestDto)
        {
            var user = await _service.Register(requestDto);
            if (!user)
                return BadRequest("Username already in use.");
            
            return Ok("User registered successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            var userLogged = await _service.Login(requestDto);
            if (!userLogged)
            {
                return Unauthorized("Invalid credentials, check if user exist y/o password match");
            }

            var token = _jwtService.GenerateToken(requestDto.Username);
            return Ok(new { token });
        }

    }
}
