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

        //Get all courses
        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all courses: {ex.Message}", ex);
            }
        }

        // Get course by ID
        public async Task<CourseDTO> GetCourseByIdAsync(int id)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);

                if (course == null)
                    throw new KeyNotFoundException($"Course with ID {id} not found.");

                return new CourseDTO
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    Description = course.Description,
                    Credits = course.Credits
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching course by ID {id}: {ex.Message}", ex);
            }
        }

        // Create a new course
        public async Task<CourseDTO> CreateCourseAsync(CourseDTO courseDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(courseDTO.Title))
                    throw new ArgumentException("Title is required.");

                if (courseDTO.Credits <= 0)
                    throw new ArgumentException("Credits must be greater than 0.");

                if (await _context.Courses.AnyAsync(c => c.Title == courseDTO.Title))
                    throw new InvalidOperationException($"A course with the title '{courseDTO.Title}' already exists.");

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
            catch (Exception ex)
            {
                throw new Exception($"Error creating course: {ex.Message}", ex);
            }
        }

        // Update course
        public async Task<bool> UpdateCourseAsync(int id, CourseDTO courseDTO)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                    throw new KeyNotFoundException($"Course with ID {id} not found.");

                if (string.IsNullOrWhiteSpace(courseDTO.Title))
                    throw new ArgumentException("Title is required.");

                if (courseDTO.Credits <= 0)
                    throw new ArgumentException("Credits must be greater than 0.");

                course.Title = courseDTO.Title;
                course.Description = courseDTO.Description;
                course.Credits = courseDTO.Credits;

                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating course with ID {id}: {ex.Message}", ex);
            }
        }

        // Delete course
        public async Task<bool> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                    throw new KeyNotFoundException($"Course with ID {id} not found.");

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting course with ID {id}: {ex.Message}", ex);
            }
        }
    }
}
