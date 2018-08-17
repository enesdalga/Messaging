using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Messaging.API.Dtos;
using Messaging.API.Models;
using Messaging.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Messaging.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid model for Register");
                return BadRequest(ModelState);
            }

            userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _authRepository.UserExistsAsync(userRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userRegisterDto.Username
            };

            var createdUser = await _authRepository.RegisterAsync(userToCreate, userRegisterDto.Password);
            if (createdUser != null)
            {
                _logger.LogInformation("New user registered");
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("User could not be register");
                return BadRequest("could not be register");
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _authRepository.LoginAsync(userLoginDto.Username.ToLower(), userLoginDto.Password);

            if (user == null){
                _logger.LogInformation("Invalid Login");
                return BadRequest("User not found");
            }
            

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesciptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expires = tokenHandler.TokenLifetimeInMinutes
            });
        }
    }
}