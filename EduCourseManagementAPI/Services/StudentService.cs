/*using Microsoft.EntityFrameworkCore;
using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.Models;

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
            var students = await _context.Students.ToListAsync();

            return students.Select(s => new StudentDTO
            {
                StudentId = s.StudentId,
                Name = s.Name,
                Email = s.Email
            });
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return null;

            return new StudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email
            };
        }

        public async Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO)
        {
            if (string.IsNullOrWhiteSpace(studentDTO.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(studentDTO.Email))
                throw new ArgumentException("Email is required.");

            if (await _context.Students.AnyAsync(s => s.Email == studentDTO.Email))
                throw new InvalidOperationException("A student with this email already exists.");

            var student = new Student
            {
                Name = studentDTO.Name,
                Email = studentDTO.Email
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            studentDTO.StudentId = student.StudentId;
            return studentDTO;
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDTO studentDTO)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            student.Name = studentDTO.Name;
            student.Email = studentDTO.Email;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
*/
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
            var students = await _context.Students.Include(s => s.User).ToListAsync();

            return students.Select(s => new StudentDTO
            {
                StudentId = s.StudentId,
                Name = s.Name,
                Email = s.Email
            });
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                return null;

            return new StudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email
            };
        }

        public async Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO)
        {
            if (string.IsNullOrWhiteSpace(studentDTO.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(studentDTO.Email))
                throw new ArgumentException("Email is required.");

            if (await _context.Students.AnyAsync(s => s.Email == studentDTO.Email))
                throw new InvalidOperationException("A student with this email already exists.");

            var student = new Student
            {
                Name = studentDTO.Name,
                Email = studentDTO.Email
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            studentDTO.StudentId = student.StudentId;
            return studentDTO;
        }

        public async Task<StudentDTO> CreateStudentWithUserAsync(int userId, StudentDTO studentDTO)
        {
            if (string.IsNullOrWhiteSpace(studentDTO.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(studentDTO.Email))
                throw new ArgumentException("Email is required.");

            if (await _context.Students.AnyAsync(s => s.Email == studentDTO.Email))
                throw new InvalidOperationException("A student with this email already exists.");

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

        public async Task<bool> UpdateStudentAsync(int id, StudentDTO studentDTO)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            student.Name = studentDTO.Name;
            student.Email = studentDTO.Email;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
