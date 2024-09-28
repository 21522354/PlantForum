using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantForum.Data;
using PlantForum.Data.Models;

namespace PlantForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PlanForumDBContext _context;

        public UserController(PlanForumDBContext context)
        {
            _context = context;
        }


        [HttpPost("Register")]
        public IActionResult Register(string username, string password)
        {
            var existUser = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
            if (existUser != null)
            {
                return BadRequest("This username is already exist");
            }
            var user = new User();
            user.id = Guid.NewGuid();
            user.UserName = username;       
            user.Password = password;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Registration successful");
        }
        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.Where(p => p.UserName == username && p.Password == password).FirstOrDefault();
            if(user == null)
            {
                return BadRequest("Wrong username or password");
            }
            return Ok("Login successful");
        }
    }
}
