using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
using EduCourseManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationCourseManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _context.Students
                    .Include(s => s.User)
                    .ToListAsync();

                return students.Select(s => new StudentDTO
                {
                    StudentId = s.StudentId,
                    Name = s.Name,
                    Email = s.Email
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all students: {ex.Message}", ex);
            }
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.StudentId == id);

                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                return new StudentDTO
                {
                    StudentId = student.StudentId,
                    Name = student.Name,
                    Email = student.Email
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching student with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<StudentDTO> CreateStudentWithUserAsync(int userId, StudentDTO studentDTO)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new ArgumentException($"User with ID {userId} does not exist.");

                if (user.Role != "Admin")
                    throw new ArgumentException($"User with ID {userId} does not have the 'Admin' role.");

                if (await _context.Students.AnyAsync(s => s.Email == studentDTO.Email))
                    throw new InvalidOperationException($"A student with email {studentDTO.Email} already exists.");

                var student = new Student
                {
                    UserId = userId,
                    Name = studentDTO.Name,
                    Email = studentDTO.Email
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                studentDTO.StudentId = student.StudentId;
                return studentDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating student: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDTO studentDTO)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                student.Name = studentDTO.Name;
                student.Email = studentDTO.Email;

                _context.Students.Update(student);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating student with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {id} not found.");

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting student with ID {id}: {ex.Message}", ex);
            }
        }
    }
}
