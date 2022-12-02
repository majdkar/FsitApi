using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskEmployees.Model;

using BC = BCrypt.Net.BCrypt;


namespace TaskEmployees.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

       private readonly DataContext _dbContext;

        public UsersController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Produces("application/json")]
        [HttpGet("GetListUser")]
        public ActionResult GetListUser()
        {
            var user = from users in _dbContext.TblUsers
                       select new
                       {
                           users.Id,
                           users.UserName,
                           users.Email,
                       };
            return Ok(user);
        }


        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromForm]Users user)
        {
                var adduser = new Users()
                {
                    UserName = user.UserName,
                    Password = BC.HashPassword(user.Password),
                    Email = user.Email,
                };
                 _dbContext.TblUsers.Add(adduser);
                 _dbContext.SaveChanges();
                return Ok(adduser);
        }
    }
}