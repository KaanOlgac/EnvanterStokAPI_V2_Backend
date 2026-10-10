using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterStokAPI.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EnvanterStokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EnvanterContext _context;
        private readonly IConfiguration _config;

        public AuthController(EnvanterContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = password + salt;
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
                private string GenerateJwtToken(StkUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? ""),
                new Claim(ClaimTypes.Role, user.RoleId.ToString() ?? ""),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class RegisterRequest
        {
            public int? RoleId { get; set; }
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string? FullName { get; set; }
            public string? EmailAddress { get; set; }
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var salt = Guid.NewGuid().ToString();
            var hash = HashPassword(request.Password, salt);

            var newUser = new StkUser
            {
                RoleId = request.RoleId,
                Username = request.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                FullName = request.FullName,
                EmailAddress = request.EmailAddress,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.StkUsers.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı oluşturuldu", userId = newUser.UserId });
        }

        public class LoginRequest
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.StkUsers
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || user.PasswordSalt == null || user.PasswordHash == null)
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });

            var hashOfEnteredPassword = HashPassword(request.Password, user.PasswordSalt);

            if (hashOfEnteredPassword != user.PasswordHash)
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
            
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Giriş başarılı",
                userId = user.UserId,
                username = user.Username,
                fullName = user.FullName,
                roleId = user.RoleId,
                token = token
            });
        }
    }
}