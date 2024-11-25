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

        [HttpGet]
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            return schedule == null ? NotFound() : Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScheduleResponse>> PostSchedule(ScheduleDTO scheduleDTO)
        {
            var response = await _scheduleService.CreateScheduleAsync(scheduleDTO);
            return response.Success ? CreatedAtAction(nameof(GetSchedule), new { id = response.CreatedSchedule.ScheduleId }, response) : Conflict(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO scheduleDTO)
        {
            var updated = await _scheduleService.UpdateScheduleAsync(id, scheduleDTO);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var deleted = await _scheduleService.DeleteScheduleAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
