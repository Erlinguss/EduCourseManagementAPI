using EducationCourseManagement.DTOs;

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
        Task<List<string>> ValidateScheduleConflictsAsync();

    }
}
