using EducationCourseManagement.DTOs;

namespace EduCourseManagementAPI.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
        Task<StudentDTO> GetStudentByIdAsync(int id);
        Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO);
        Task<StudentDTO> CreateStudentWithUserAsync(int userId, StudentDTO studentDTO);
        Task<bool> UpdateStudentAsync(int id, StudentDTO studentDTO);
        Task<bool> DeleteStudentAsync(int id);
    }
}
