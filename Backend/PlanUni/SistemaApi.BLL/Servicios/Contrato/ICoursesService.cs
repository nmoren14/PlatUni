using SistemaApi.DTO;
using SistemaApi.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaApi.BLL.Servicios.Contrato
{
    public interface ICourseService
    {
        Task<List<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDTO> GetCourseByIdAsync(int courseId);
        Task<CourseDTO> CreateCourseAsync(CourseDTO courseDTO);
        Task<CourseDTO> UpdateCourseAsync(int courseId, CourseDTO courseDTO);
        Task<bool> DeleteCourseAsync(int courseId);
    }
}
