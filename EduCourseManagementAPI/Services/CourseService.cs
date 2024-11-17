using Microsoft.EntityFrameworkCore;
using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;

namespace EducationCourseManagement.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;

        public CourseService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();

            return courses.Select(c => new CourseDTO
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits
            });
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return null;

            return new CourseDTO
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Credits = course.Credits
            };
        }

        public async Task<CourseDTO> CreateCourseAsync(CourseDTO courseDTO)
        {
            var course = new Course
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                Credits = courseDTO.Credits
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            courseDTO.CourseId = course.CourseId; 
            return courseDTO;
        }

        public async Task<bool> UpdateCourseAsync(int id, CourseDTO courseDTO)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;

            course.Title = courseDTO.Title;
            course.Description = courseDTO.Description;
            course.Credits = courseDTO.Credits;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
