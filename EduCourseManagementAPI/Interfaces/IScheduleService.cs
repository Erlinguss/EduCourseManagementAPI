using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;

namespace EduCourseManagementAPI.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDTO>> GetAllSchedulesAsync();
        Task<ScheduleDTO> GetScheduleByIdAsync(int id);
        Task<ScheduleDTO> CreateScheduleAsync(ScheduleDTO scheduleDTO);
        Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO);
        Task<bool> DeleteScheduleAsync(int id);


        Task<bool> GenerateSchedulesForDayAsync(DateTime date);
        Task<ScheduleDTO> ValidatedScheduleAsync();

       /* Task<ScheduleDTO> ValidatedScheduleAsync(int courseId, int instructorId, int roomId, DateTime date, string timeSlot);
      */

    }

}
