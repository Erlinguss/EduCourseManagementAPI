using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduCourseManagementAPI.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDTO>> GetAllSchedulesAsync();
        Task<ScheduleDTO> GetScheduleByIdAsync(int id);
        Task<ScheduleResponse> CreateScheduleAsync(ScheduleDTO scheduleDTO);
        Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO);
        Task<bool> DeleteScheduleAsync(int id);
        Task<(bool IsConflict, string Message, Schedule ExistingSchedule, List<string> SuggestedTimeSlots)> ValidatedScheduleAsync(
            int courseId, int instructorId, int roomId, DateTime date, string timeSlot);
    }
}
