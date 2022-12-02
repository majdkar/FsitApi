using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskEmployees.Model;
using BC = BCrypt.Net.BCrypt;

namespace TaskEmployees.Controllers
{
    [Route("api/Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DataContext _dbContext;

        public TokenController(IConfiguration  configuration , DataContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Produces("application/json")]
        [HttpPost("LoginUser")]
        public async Task<ActionResult> LoginUser(Users users)
        {
            if(users != null && users.Password != null && users.Email != null)
            {
                var user = await CheckUser(users.Email, users.Password);
                if(user != null)
                {
                    var claims = new[]
                    {
                      new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id" , user.Id.ToString()),
                        new Claim("UserName" , user.UserName),
                        new Claim("Email" , user.Email),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"]
                        , expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Email And Passwrod");
                }
                
            }
            else
            {
                return BadRequest("Invalid Email And Passwrod");
            }
        } 

        private async Task<Users> CheckUser(string email , string password)
        {
            var user = await _dbContext.TblUsers.FirstOrDefaultAsync(x => x.Email == email);

            if(user != null && BC.Verify(password, user.Password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}