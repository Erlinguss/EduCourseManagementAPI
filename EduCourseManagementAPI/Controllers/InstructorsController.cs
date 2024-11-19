/*using Microsoft.AspNetCore.Mvc;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.DTOs;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        // GET: api/Instructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorDTO>>> GetInstructors()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            return Ok(instructors);
        }

        // GET: api/Instructors/id
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDTO>> GetInstructor(int id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);

            if (instructor == null) return NotFound();

            return Ok(instructor);
        }

        // POST: api/Instructors
        [HttpPost]
        public async Task<ActionResult<InstructorDTO>> PostInstructor(InstructorDTO instructorDTO)
        {
            var createdInstructor = await _instructorService.CreateInstructorAsync(instructorDTO);
            return CreatedAtAction(nameof(GetInstructor), new { id = createdInstructor.InstructorId }, createdInstructor);
        }

        // PUT: api/Instructors/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor(int id, InstructorDTO instructorDTO)
        {
            var updated = await _instructorService.UpdateInstructorAsync(id, instructorDTO);

            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/Instructors/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var deleted = await _instructorService.DeleteInstructorAsync(id);

            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.DTOs;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure that all actions require authentication by default
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

        // GET: api/Instructors/id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Instructor")] 
        public async Task<ActionResult<InstructorDTO>> GetInstructor(int id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);

            if (instructor == null) return NotFound();

            return Ok(instructor);
        }

        // POST: api/Instructors
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<InstructorDTO>> PostInstructor(InstructorDTO instructorDTO)
        {
            var createdInstructor = await _instructorService.CreateInstructorAsync(instructorDTO);
            return CreatedAtAction(nameof(GetInstructor), new { id = createdInstructor.InstructorId }, createdInstructor);
        }

        // PUT: api/Instructors/id
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> PutInstructor(int id, InstructorDTO instructorDTO)
        {
            var updated = await _instructorService.UpdateInstructorAsync(id, instructorDTO);

            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/Instructors/id
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var deleted = await _instructorService.DeleteInstructorAsync(id);

            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
