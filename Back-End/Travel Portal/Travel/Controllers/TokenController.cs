using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travel.Model;
using Travel.DB;
using Travel.Model;

namespace Trave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly TravelContext _context;
        private const string AgentRole = "Agent";
        private const string AdminRole = "Admin";
        private const string UserRole = "User";

        public TokensController(IConfiguration config, TravelContext context)
        {
            _configuration = config;
            _context = context;
        }
        [HttpPost("TravelAgent")]
        public async Task<IActionResult> Post(TravelAgent _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetTravelAgents(_userData.Email, _userData.Password);

                if (user != null)
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("TravelId", user.TravelId.ToString()),
                    new Claim("Email", user.Email),
                    new Claim(ClaimTypes.Role, AgentRole)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<TravelAgent> GetTravelAgents(string email, string password)
        {
            return await _context.TravelAgents.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        [HttpPost("Admin")]
        public async Task<IActionResult> Post(Admin _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetAdmins(_userData.Email, _userData.Password);

                if (user != null && user.AdminId != null && user.Email != null)
                {
                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("AdminId", user.AdminId.ToString()),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, AdminRole)
            };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    if (user == null)
                    {
                        // Add logging to identify if the user is null
                        Console.WriteLine("User is null");
                    }
                    else if (user.AdminId == null)
                    {
                        // Add logging to identify if AdminId is null
                        Console.WriteLine("AdminId is null");
                    }
                    else if (user.Email == null)
                    {
                        // Add logging to identify if Email is null
                        Console.WriteLine("Email is null");
                    }

                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }


        private async Task<Admin> GetAdmins(string email, string password)
        {
            return await _context.Admins.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

        }

        [HttpPost("User")]
        public async Task<IActionResult> Post(User userViewModel)
        {
            if (userViewModel != null && userViewModel.Email != null && userViewModel.Password != null)
            {
                var user = await GetUser(userViewModel.Email, userViewModel.Password);

                if (user != null && user.UserId != null && user.Email != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Email", user.Email),
                        new Claim(ClaimTypes.Role, UserRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}