using EducationCourseManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationCourseManagement.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Instructor>().ToTable("Instructors");
            modelBuilder.Entity<Schedule>().ToTable("Schedules");
        }
    }
}

