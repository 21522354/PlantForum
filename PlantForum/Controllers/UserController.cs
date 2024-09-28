using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantForum.Data;
using PlantForum.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlantForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PlanForumDBContext _context;
        private readonly IConfiguration _configuration;

        public UserController(PlanForumDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 
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
            var token = GenerateToken(user);    
            return Ok(token);
        }
        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.Where(p => p.UserName == username && p.Password == password).FirstOrDefault();
            if(user == null)
            {
                return BadRequest("Wrong username or password");
            }
            var token = GenerateToken(user);
            return Ok(token);
        }

        private string GenerateToken(User user)
        {
            // phát sinh token và trả về cho người dùng sau khi đăng nhập thành công
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["AppSettings:SecretKey"];
            var secterKeyByte = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // nội dung của token   
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserID", user.id.ToString()),
                    // role
                }),
                // thời gian sống của token
                Expires = DateTime.UtcNow.AddHours(1),
                // ký vào token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secterKeyByte), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var accessToken = jwtTokenHandler.WriteToken(token);
            return accessToken;

        }
    }
}
