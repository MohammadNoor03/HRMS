using HRMS.DbContexts;
using HRMS.Dtos.Auth;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HMRSContext _dbContext;

        public AuthController(HMRSContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            // Admin == admin => true
            var user = _dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == loginDto.UserName.ToUpper());
            if (user == null)
            {
                return Unauthorized("Invalid Username Or Password"); // 401
            }

            // password -> Admin@123 == $2a$11$2sHodg5MPx0S7Q5mxSIhOeo.M/Wvh7ZabN4b1IS8oiNgGFh6dZrDC => Salt, Cost Factor
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
            {
                return Unauthorized("Invalid Username Or Password"); // 401
            }

            // Token => JwtBearer
            var token = GenerateJwtToken(user);

            return Ok(token);

        }
        // Token
        private string GenerateJwtToken(User user)
        {
            // Claims => User Info
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())); // Key / Value
            claims.Add(new Claim(ClaimTypes.Name, user.Username)); // Key / Value

            // Role => Admin, HR, Developer...
            if (user.IsAdmin)
            {
                // Admin
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                // Employee => Position
            }

            
            // Secert Key + Signing Token
            // WHAFWEI#!@S!!112312WQEQW@RWQEQW432
            // [68, 55, 31...]
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token Settings
            var tokenSettings = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(1)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSettings);

            return token;
           

        }


    }
}
