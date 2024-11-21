using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.DTOs;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        // GET: api/Instructors
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<IEnumerable<InstructorDTO>>> GetInstructors()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            return Ok(instructors);
        }

        // GET: api/Instructors/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<InstructorDTO>> GetInstructor(int id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);

            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        // POST: api/Instructors
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InstructorDTO>> PostInstructorWithUser(int userId, [FromBody] InstructorDTO instructorDTO)
        {
            try
            {
                var createdInstructor = await _instructorService.CreateInstructorWithUserAsync(userId, instructorDTO);
                return CreatedAtAction(nameof(GetInstructor), new { id = createdInstructor.InstructorId }, createdInstructor);
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

        // PUT: api/Instructors/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutInstructor(int id, [FromBody] InstructorDTO instructorDTO)
        {
            var updated = await _instructorService.UpdateInstructorAsync(id, instructorDTO);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Instructors/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var deleted = await _instructorService.DeleteInstructorAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
