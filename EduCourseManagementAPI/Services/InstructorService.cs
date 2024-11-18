using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationCourseManagement.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly SchoolContext _context;

        public InstructorService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InstructorDTO>> GetAllInstructorsAsync()
        {
            var instructors = await _context.Instructors.ToListAsync();

            return instructors.Select(i => new InstructorDTO
            {
                InstructorId = i.InstructorId,
                Name = i.Name,
                Email = i.Email
            });
        }

        public async Task<InstructorDTO> GetInstructorByIdAsync(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null) return null;

            return new InstructorDTO
            {
                InstructorId = instructor.InstructorId,
                Name = instructor.Name,
                Email = instructor.Email
            };
        }

        
    }
}
