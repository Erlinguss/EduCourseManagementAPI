using Microsoft.AspNetCore.Mvc;
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


    }
}
