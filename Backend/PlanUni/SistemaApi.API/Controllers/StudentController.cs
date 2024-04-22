using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaApi.BLL.Servicios.Contrato;
using SistemaApi.DTO;
using SistemaApi.API.Utilidad;
using SistemaApi.Model.Models;
namespace SistemaApi.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost("CreateStudent")]
        public async Task<ActionResult<StudentDTO>> CreateStudent(StudentDTO studentDTO)
        {
            var createdStudent = await _studentService.CreateStudentAsync(studentDTO);
            return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.StudentId }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentDTO studentDTO)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(id, studentDTO);
            if (updatedStudent == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("EnrollStudent")]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentDTO enrollmentDTO)
        {
            if (enrollmentDTO == null)
            {
                return BadRequest("Datos de inscripción no válidos");
            }

            var result = await _studentService.EnrollStudentAsync(enrollmentDTO);

            if (result)
            {
                return Ok("El estudiante ha sido inscrito correctamente");
            }
            else
            {
                return BadRequest("No se pudo realizar la inscripción");
            }
        }

        [HttpGet("{studentId}/materias")]
        public async Task<IActionResult> GetEnrolledCourses(int studentId)
        {
            var enrolledCourses = await _studentService.GetEnrolledCoursesAsync(studentId);

            if (enrolledCourses == null || enrolledCourses.Count == 0)
            {
                return NotFound($"No se encontraron materias inscritas para el estudiante con ID {studentId}");
            }

            return Ok(enrolledCourses);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
