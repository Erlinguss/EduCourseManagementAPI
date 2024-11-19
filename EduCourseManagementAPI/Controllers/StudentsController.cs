/*using Microsoft.AspNetCore.Mvc;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDTO)
        {
            var createdStudent = await _studentService.CreateStudentAsync(studentDTO);
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.StudentId }, createdStudent);
        }

        // PUT: api/Students/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentDTO studentDTO)
        {
            var updated = await _studentService.UpdateStudentAsync(id, studentDTO);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Students/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleted = await _studentService.DeleteStudentAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}

*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensures all actions require authentication by default
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Student")] 
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDTO)
        {
            var createdStudent = await _studentService.CreateStudentAsync(studentDTO);
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.StudentId }, createdStudent);
        }

        // PUT: api/Students/id
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> PutStudent(int id, StudentDTO studentDTO)
        {
            var updated = await _studentService.UpdateStudentAsync(id, studentDTO);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Students/id
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
