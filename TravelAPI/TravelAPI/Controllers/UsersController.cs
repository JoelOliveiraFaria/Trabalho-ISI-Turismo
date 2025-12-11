using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TravelAPI.Data;
using Microsoft.AspNetCore.Identity;
using TravelAPI.DTOs;
using TravelAPI.Models;
using TravelAPI.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace TravelAPI.Controllers
{
    [Route("travel/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TravelPlannerContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(TravelPlannerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string CreateToken(User user)
        {
           List<Claim> claims = new List<Claim>
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Email, user.Email!)
           };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        //Registrar novo usuário
        //POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserRegisterDTO registerDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
            {
                return BadRequest();
            }

            var user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email
            };

            var passwordHasher = new PasswordHasher<User>();

            user.PasswordHash = passwordHasher.HashPassword(user, registerDTO.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDTO loginDTO)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginDTO.Username);

            if (user == null) return Unauthorized();

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, loginDTO.Password!);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        //// 3. OBTER TODOS (Apenas para teste/Admin)
        //// GET: api/users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        //{
        //    var users = await _context.Users
        //        .Select(u => new UserDTO
        //        {
        //            Id = u.Id,
        //            Username = u.Username,
        //            Email = u.Email
        //        })
        //        .ToListAsync();

        //    return Ok(users);
        //}


        //Obter usuário por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
