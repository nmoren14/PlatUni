using SistemaApi.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApi.DAL.Repositorios.Contrato
{
    public interface IStudentRepository:IGenericRepository<Student>
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<int> GetEnrolledCourseCountAsync(int studentId);
        Task<List<Course>> GetStudentCoursesAsync(int studentId);
        Task<bool> IsStudentAlreadyEnrolledAsync(int studentId, int courseId);
        Task<bool> IsStudentCourseLimitExceededAsync(int studentId);
        Task<bool> IsStudentEnrolledWithSameProfessorAsync(int studentId, int professorId);
        Task<List<Enrollment>> GetStudentEnrollmentsAsync(int studentId);
    }
}
