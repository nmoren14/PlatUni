using Microsoft.EntityFrameworkCore;
using SistemaApi.DAL.DBContext;
using SistemaApi.DAL.Repositorios.Contrato;
using SistemaApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApi.DAL.Repositorios
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly PlatUniContext _context;

        public StudentRepository(PlatUniContext context): base(context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
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

        public async Task<int> GetEnrolledCourseCountAsync(int studentId)
        {
            return await _context.Enrollments.CountAsync(e => e.StudentId == studentId);
        }

        public async Task<List<Course>> GetStudentCoursesAsync(int studentId)
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            return enrollments.Select(e => e.Course).ToList();
        }

        public async Task<bool> IsStudentAlreadyEnrolledAsync(int studentId, int courseId)
        {
            return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<bool> IsStudentCourseLimitExceededAsync(int studentId)
        {
            return await _context.Enrollments.CountAsync(e => e.StudentId == studentId) >= 3;
        }

        public async Task<bool> IsStudentEnrolledWithSameProfessorAsync(int studentId, int professorId)
        {
            return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.ProfessorId == professorId);
        }

        public async Task<List<Enrollment>> GetStudentEnrollmentsAsync(int studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Professor)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }
    }
}
