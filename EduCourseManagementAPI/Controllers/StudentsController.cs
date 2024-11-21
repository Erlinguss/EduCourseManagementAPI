using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.DTOs;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        // GET: api/Students/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // POST: api/Students?userId=
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDTO>> PostStudentWithUser(int userId, [FromBody] StudentDTO studentDTO)
        {
            try
            {
                var createdStudent = await _studentService.CreateStudentWithUserAsync(userId, studentDTO);
                return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.StudentId }, createdStudent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // PUT: api/Students/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] StudentDTO studentDTO)
        {
            var updated = await _studentService.UpdateStudentAsync(id, studentDTO);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Students/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleted = await _studentService.DeleteStudentAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
