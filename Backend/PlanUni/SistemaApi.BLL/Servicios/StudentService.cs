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
    public class StudentService : IStudentService
    {
        private readonly PlatUniContext _context;
        private readonly IMapper _mapper;

        public StudentService(PlatUniContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();
            var studentDTOs = _mapper.Map<List<StudentDTO>>(students); // Mapear a StudentDTO

            // Opcional: Asignar la cantidad de materias inscritas a cada DTO
            var enrolledCourseCounts = await _context.Enrollments
                .GroupBy(e => e.StudentId)
                .Select(g => new { StudentId = g.Key, Count = g.Count() })
                .ToListAsync();

            var enrolledCourseCountsDict = enrolledCourseCounts.ToDictionary(e => e.StudentId, e => e.Count);

            foreach (var studentDto in studentDTOs)
            {
                if (enrolledCourseCountsDict.TryGetValue(studentDto.StudentId, out int count))
                {
                    studentDto.EnrollmentCount = count;
                }
                else
                {
                    studentDto.EnrollmentCount = 0;
                }
            }

            return studentDTOs;
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return null; // Estudiante no encontrado
            }

            // Mapear a StudentDTO
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return studentDTO;
        }

        public async Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO)
        {
            // Mapear desde StudentDTO a Student utilizando AutoMapper
            var student = _mapper.Map<Student>(studentDTO);

            // Agregar el nuevo estudiante a la base de datos
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Mapear de nuevo a StudentDTO para devolver como resultado
            var createdStudentDTO = _mapper.Map<StudentDTO>(student);
            return createdStudentDTO;
        }

        public async Task<StudentDTO> UpdateStudentAsync(int studentId, StudentDTO studentDTO)
        {
            var existingStudent = await _context.Students.FindAsync(studentId);
            if (existingStudent == null)
            {
                return null;
            }

            // Actualizar el estudiante existente con los datos del DTO
            _mapper.Map(studentDTO, existingStudent);

            try
            {
                await _context.SaveChangesAsync();

                // Mapear de nuevo a StudentDTO para devolver como resultado
                var updatedStudentDTO = _mapper.Map<StudentDTO>(existingStudent);
                return updatedStudentDTO;
            }
            catch (DbUpdateConcurrencyException)
            {
                return null; 
            }
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return false; 
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnrollStudentAsync(EnrollmentDTO enrollmentDTO)
        {
            var studentId = enrollmentDTO.StudentId;
            var courseId = enrollmentDTO.CourseId;
            var professorId = enrollmentDTO.ProfessorId;

            // Validar existencia del estudiante, curso y profesor
            var studentExists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
            var courseExists = await _context.Courses.AnyAsync(c => c.CourseId == courseId);
            var professorExists = await _context.Professors.AnyAsync(p => p.ProfessorId == professorId);

            if (!studentExists || !courseExists || !professorExists)
            {
                return false;
            }

            // Verificar si el estudiante ya está inscrito en el curso
            var existingEnrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
            if (existingEnrollment != null)
            {
                return false; // El estudiante ya está inscrito en este curso
            }

            // Verificar el límite de 3 materias inscritas
            var enrolledCourseCount = await _context.Enrollments.CountAsync(e => e.StudentId == studentId);
            if (enrolledCourseCount >= 3)
            {
                return false; // Límite de materias inscritas alcanzado
            }

            // Verificar si el estudiante ya tiene materias inscritas con el mismo profesor
            var existingEnrollmentsWithProfessor = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.ProfessorId == professorId);
            if (existingEnrollmentsWithProfessor)
            {
                return false; // El estudiante ya está inscrito con este profesor
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                ProfessorId = professorId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<CourseDTO>> GetEnrolledCoursesAsync(int studentId)
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            var courses = enrollments.Select(e => e.Course);

            return _mapper.Map<List<CourseDTO>>(courses);
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}