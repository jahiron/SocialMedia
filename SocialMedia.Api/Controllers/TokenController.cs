using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/values
        [HttpPost]
        public IActionResult GetToken(UserDto user)
        {
            if (IsValidUser(user))
            {
                string token = GenerateToken(user);
                return Ok(token);
            }

            return BadRequest();
        }

        private string GenerateToken(UserDto user)
        {
            //Header
            var symetricSecutityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Secret"]));
            var signalCredentials = new SigningCredentials(symetricSecutityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signalCredentials);

            //Payload
            var claims = new[]
            {
                new Claim(user.FirstName,user.FirstName),
                new Claim(user.LastName,user.LastName),
                new Claim(user.Email,user.Email),
            };

            //Signature
            var jwtPayload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(5)
            );

            var token = new JwtSecurityToken(header, jwtPayload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidUser(UserDto user)
        {
            return true;
        }
    }
}
