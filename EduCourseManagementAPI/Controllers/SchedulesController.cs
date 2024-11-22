using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.DTOs;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET: api/Schedules
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        // GET: api/Schedules/id
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);

            if (schedule == null) return NotFound();

            return Ok(schedule);
        }

        // POST: api/Schedules
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO scheduleDTO)
        {
            var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleDTO);
            return CreatedAtAction(nameof(GetSchedule), new { id = createdSchedule.ScheduleId }, createdSchedule);
        }

        // PUT: api/Schedules/id
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO scheduleDTO)
        {
            var updated = await _scheduleService.UpdateScheduleAsync(id, scheduleDTO);

            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/Schedules/id
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var deleted = await _scheduleService.DeleteScheduleAsync(id);

            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateSchedules([FromQuery] DateTime date)
        {
            if (date == default)
                return BadRequest("Invalid date provided.");

            var success = await _scheduleService.GenerateSchedulesForDayAsync(date);
            if (success)
                return Ok($"Schedules successfully generated for {date:yyyy-MM-dd}.");
            return StatusCode(500, "Failed to generate schedules.");
        }

    }
}
