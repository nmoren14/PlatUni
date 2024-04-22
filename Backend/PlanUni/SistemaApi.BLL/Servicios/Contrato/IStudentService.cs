using SistemaApi.DTO;
using SistemaApi.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaApi.BLL.Servicios.Contrato
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAllStudentsAsync();
        Task<StudentDTO> GetStudentByIdAsync(int studentId);
        Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO);
        Task<StudentDTO> UpdateStudentAsync(int studentId, StudentDTO studentDTO);
        Task<bool> DeleteStudentAsync(int studentId);
        Task<bool> EnrollStudentAsync(EnrollmentDTO enrollmentDTO);
        Task<List<CourseDTO>> GetEnrolledCoursesAsync(int studentId);


    }
}
