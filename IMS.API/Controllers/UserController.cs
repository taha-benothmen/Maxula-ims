using IMS.Shared.Data;
using IMS.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Sign Up
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { Message = "User already exists with this email." });
            }

            // Hash the password
            var hashedPassword = HashPassword(request.Password);

            // Create a new user
            var user = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                FullName = request.FullName
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully." });
        }

        // Sign In
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequest request)
        {
            // Check if user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // Success: return a response (could be a token or just a success message)
            return Ok(new { Message = "Login successful", UserId = user.Id });
        }

        // Utility: Hash password
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Utility: Verify password
        private static bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
