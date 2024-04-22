using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaApi.BLL.Servicios.Contrato;
using SistemaApi.DAL.DBContext;
using SistemaApi.DTO;
using SistemaApi.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaApi.BLL.Servicios
{
    public class CourseService : ICourseService
    {
        private readonly PlatUniContext _context;
        private readonly IMapper _mapper;

        public CourseService(PlatUniContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            var courseDTOs = _mapper.Map<List<CourseDTO>>(courses);
            return courseDTOs;
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var courseDTO = _mapper.Map<CourseDTO>(course);
            return courseDTO;
        }

        public async Task<CourseDTO> CreateCourseAsync(CourseDTO courseDTO)
        {
            var course = _mapper.Map<Course>(courseDTO);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            var createdCourseDTO = _mapper.Map<CourseDTO>(course);
            return createdCourseDTO;
        }

        public async Task<CourseDTO> UpdateCourseAsync(int courseId, CourseDTO courseDTO)
        {
            var existingCourse = await _context.Courses.FindAsync(courseId);
            if (existingCourse == null)
            {
                return null;
            }

            _mapper.Map(courseDTO, existingCourse);
            await _context.SaveChangesAsync();

            var updatedCourseDTO = _mapper.Map<CourseDTO>(existingCourse);
            return updatedCourseDTO;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
