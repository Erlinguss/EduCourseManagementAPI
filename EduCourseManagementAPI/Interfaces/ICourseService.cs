using EducationCourseManagement.DTOs;

namespace EduCourseManagementAPI.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDTO> GetCourseByIdAsync(int id);
        Task<CourseDTO> CreateCourseAsync(CourseDTO courseDTO);
        Task<bool> UpdateCourseAsync(int id, CourseDTO courseDTO);
        Task<bool> DeleteCourseAsync(int id);
    }
}