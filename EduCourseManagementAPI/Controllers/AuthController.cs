/*using Microsoft.AspNetCore.Mvc;
using EducationCourseManagement.Services;
using EducationCourseManagement.Models;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User request)
        {
            // Mock user validation with role
            if (request.Username == "PeterAdmin" && request.Password == "admindkit" && request.Role == "Admin")
            {
                var token = _tokenService.GenerateToken(request.Role);
                return Ok(new { Token = token });
            }
            else if (request.Username == "JohnInstructor" && request.Password == "instructordkit" && request.Role == "Instructor")
            {
                var token = _tokenService.GenerateToken(request.Role);
                return Ok(new { Token = token });
            }
            else if (request.Username == "DavidStudent" && request.Password == "studentdkit" && request.Role == "Student")
            {
                var token = _tokenService.GenerateToken(request.Role);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials or role.");
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationCourseManagement.Services;
using EducationCourseManagement.Models;
using EducationCourseManagement.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly SchoolContext _context;

        public AuthController(TokenService tokenService, SchoolContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        // Login Method
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User request)
        {
            // Retrieve the user from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || user.Password != request.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate token based on the user's role
            var token = _tokenService.GenerateToken(user.Role);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register request)
        {
            // Check if the username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            // Create a new User
            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password,
                Role = request.Role
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            // Insert into Students or Instructors based on Role
            if (request.Role == "Student")
            {
                var newStudent = new Student
                {
                    UserId = newUser.UserId, 
                    Name = request.Name,
                    Email = request.Email
                };

                await _context.Students.AddAsync(newStudent);
            }
            else if (request.Role == "Instructor")
            {
                var newInstructor = new Instructor
                {
                    UserId = newUser.UserId, 
                    Name = request.Name,
                    Email = request.Email
                };

                await _context.Instructors.AddAsync(newInstructor);
            }
            else
            {
                return BadRequest("Invalid role specified. Role must be 'Student' or 'Instructor'.");
            }

            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

    }
}




