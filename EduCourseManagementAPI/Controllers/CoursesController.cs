using Microsoft.AspNetCore.Mvc;
using EducationCourseManagement.Services;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> PostCourse(CourseDTO courseDTO)
        {
            var createdCourse = await _courseService.CreateCourseAsync(courseDTO);
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.CourseId }, createdCourse);
        }

        // PUT: api/Courses/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseDTO courseDTO)
        {
            var updated = await _courseService.UpdateCourseAsync(id, courseDTO);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Courses/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var deleted = await _courseService.DeleteCourseAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
