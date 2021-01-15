using DockerCore.Cross.Entities;
using DockerCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace DockerCore.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>  
        /// Login Authenticaton using JWT Token Authentication  
        /// </summary>  
        /// <param name="loginModel"></param>  
        /// <returns>Result authenticate</returns>  
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = AuthenticateUser(loginModel);

                if (user != null)
                {
                    var tokenString = GenerateJsonWebToken();
                    response = Ok(new { Token = tokenString, Message = "Success" });
                }

                return response;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>  
        /// Hardcoded the User authentication  
        /// </summary>  
        /// <param name="loginModel"></param>  
        /// <returns>The user information</returns>  
        private User AuthenticateUser(LoginModel loginModel)
        {
            User user = null;
            user = new User { Name = "Test Roulette", Balance = 30000 };
            
            return user;
        }

        /// <summary>  
        /// Generate Json Web Token Method  
        /// </summary>  
        /// <returns>The token</returns>  
        private string GenerateJsonWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _configuration["JwtConfig:Issuer"], audience: _configuration["JwtConfig:Audience"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
