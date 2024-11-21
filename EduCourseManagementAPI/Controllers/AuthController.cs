using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationCourseManagement.Services;
using EducationCourseManagement.Models;
using EducationCourseManagement.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;

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

            // Validate role
            if (!new[] { "Admin", "Instructor", "Student" }.Contains(request.Role))
            {
                return BadRequest(new { Message = "Invalid role specified. Role must be 'Admin', 'Instructor', or 'Student'." });
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

            if (request.Role == "Admin")
            {
                return Ok(new { UserId = newUser.UserId, Message = "Admin registered successfully." });
            }

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

            // Return the userId in the response
            return Ok(new
            {
                message = "User registered successfully.",
                userId = newUser.UserId
            });
        }

    }
}





