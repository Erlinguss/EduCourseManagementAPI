using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
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
            var instructors = await _context.Instructors
                .Include(i => i.User) 
                .ToListAsync();

            return instructors.Select(i => new InstructorDTO
            {
                InstructorId = i.InstructorId,
                Name = i.Name,
                Email = i.Email
            });
        }

        public async Task<InstructorDTO> GetInstructorByIdAsync(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.User) 
                .FirstOrDefaultAsync(i => i.InstructorId == id);

            if (instructor == null)
                return null;

            return new InstructorDTO
            {
                InstructorId = instructor.InstructorId,
                Name = instructor.Name,
                Email = instructor.Email
            };
        }

        public async Task<InstructorDTO> CreateInstructorWithUserAsync(int userId, InstructorDTO instructorDTO)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} does not exist.");

            if (user.Role != "Instructor")
                throw new ArgumentException($"User with ID {userId} does not have the 'Instructor' role.");

            if (await _context.Instructors.AnyAsync(i => i.Email == instructorDTO.Email))
                throw new InvalidOperationException($"An instructor with email {instructorDTO.Email} already exists.");

            var instructor = new Instructor
            {
                UserId = userId,
                Name = instructorDTO.Name,
                Email = instructorDTO.Email
            };

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            instructorDTO.InstructorId = instructor.InstructorId;
            return instructorDTO;
        }

        public async Task<bool> UpdateInstructorAsync(int id, InstructorDTO instructorDTO)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
                return false;

            instructor.Name = instructorDTO.Name;
            instructor.Email = instructorDTO.Email;

            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteInstructorAsync(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
                return false;

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
