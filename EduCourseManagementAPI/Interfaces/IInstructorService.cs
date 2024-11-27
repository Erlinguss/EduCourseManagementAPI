using EducationCourseManagement.DTOs;

namespace EduCourseManagementAPI.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorDTO>> GetAllInstructorsAsync();
        Task<InstructorDTO> GetInstructorByIdAsync(int id);
        Task<InstructorDTO> CreateInstructorWithUserAsync(int userId, InstructorDTO instructorDTO);
        Task<bool> UpdateInstructorAsync(int id, InstructorDTO instructorDTO);
        Task<bool> DeleteInstructorAsync(int id);
    }
}