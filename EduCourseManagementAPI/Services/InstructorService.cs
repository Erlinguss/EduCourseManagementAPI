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
            try
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
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all instructors: {ex.Message}", ex);
            }
        }

        public async Task<InstructorDTO> GetInstructorByIdAsync(int id)
        {
            try
            {
                var instructor = await _context.Instructors
                    .Include(i => i.User)
                    .FirstOrDefaultAsync(i => i.InstructorId == id);

                if (instructor == null)
                    throw new KeyNotFoundException($"Instructor with ID {id} not found.");

                return new InstructorDTO
                {
                    InstructorId = instructor.InstructorId,
                    Name = instructor.Name,
                    Email = instructor.Email
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching instructor by ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<InstructorDTO> CreateInstructorWithUserAsync(int userId, InstructorDTO instructorDTO)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new ArgumentException($"User with ID {userId} does not exist.");

                if (user.Role != "Admin")
                    throw new ArgumentException($"User with ID {userId} does not have the 'Admin' role.");

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
            catch (Exception ex)
            {
                throw new Exception($"Error creating instructor: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateInstructorAsync(int id, InstructorDTO instructorDTO)
        {
            try
            {
                var instructor = await _context.Instructors.FindAsync(id);
                if (instructor == null)
                    throw new KeyNotFoundException($"Instructor with ID {id} not found.");

                instructor.Name = instructorDTO.Name;
                instructor.Email = instructorDTO.Email;

                _context.Instructors.Update(instructor);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating instructor with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteInstructorAsync(int id)
        {
            try
            {
                var instructor = await _context.Instructors.FindAsync(id);
                if (instructor == null)
                    throw new KeyNotFoundException($"Instructor with ID {id} not found.");

                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting instructor with ID {id}: {ex.Message}", ex);
            }
        }
    }
}
