using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlatUni.Models;

namespace PlatUni.Controllers
{
    public class StudentsController : Controller
    {
        private readonly PlatUniContext _context;

        public StudentsController(PlatUniContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();

            // Consulta para obtener la cantidad de materias inscritas por cada estudiante
            var enrolledCourseCounts = await _context.Enrollments
                .GroupBy(e => e.StudentId)
                .Select(g => new { StudentId = g.Key, Count = g.Count() })
                .ToListAsync();

            var enrolledCourseCountsDict = enrolledCourseCounts.ToDictionary(e => e.StudentId, e => e.Count);

            ViewBag.EnrolledCourseCounts = enrolledCourseCountsDict;

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Database.OpenConnection();
                _context.Database.ExecuteSqlRaw("INSERT INTO Students (FirstName, LastName, Email) VALUES ('"+student.FirstName+ "','"+student.LastName+ "','"+student.Email+"');");
                _context.SaveChanges();
                _context.Database.CloseConnection();

                return RedirectToAction("Index");
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,Email")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student); // Mostrar la vista de confirmación con los detalles del estudiante
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Eliminar los registros de Enrollments asociados al estudiante
            var enrollments = _context.Enrollments.Where(e => e.StudentId == id);
            _context.Enrollments.RemoveRange(enrollments);

            // Finalmente, eliminar el estudiante
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }

        public async Task<IActionResult> InscribirM(int studentId)
        {
            var courses = await _context.Courses.ToListAsync();
            ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");

            ViewBag.StudentId = studentId;
            var Student = await _context.Students.FirstOrDefaultAsync(p => p.StudentId == studentId);
            ViewBag.Student = Student.FirstName +" "+ Student.LastName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InscribirM(int studentId, int courseId)
        {
            var Student = await _context.Students.FirstOrDefaultAsync(p => p.StudentId == studentId);
            
            // Verificar que el curso seleccionado exista
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                ModelState.AddModelError(string.Empty, $"El curso con ID {courseId} no existe.");
                var courses = await _context.Courses.ToListAsync();
                ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");
                ViewBag.StudentId = studentId;
                ViewBag.Student = Student.FirstName + " " + Student.LastName;
                return View();
            }

            // Obtener al estudiante desde la base de datos
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            // Obtener al profesor asociado al curso
            var professor = await _context.Professors.FirstOrDefaultAsync(p => p.FisrtClass == courseId || p.SecondClass == courseId);
            if (professor == null)
            {
                ModelState.AddModelError(string.Empty, $"No se encontró al profesor asociado al curso {course.CourseName}.");
                var courses = await _context.Courses.ToListAsync();
                ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");
                ViewBag.StudentId = studentId;
                ViewBag.Student = Student.FirstName + " " + Student.LastName;
                return View();
            }
            // Verificar si el estudiante ya está inscrito en la misma materia
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                ModelState.AddModelError(string.Empty, $"El estudiante ya está inscrito en el curso {course.CourseName}.");
                var courses = await _context.Courses.ToListAsync();
                ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");
                ViewBag.StudentId = studentId;
                ViewBag.Student = Student.FirstName + " " + Student.LastName;
                return View();
            }
            // Verificar el límite de 3 materias inscritas
            var enrolledCourseCount = await _context.Enrollments
                .CountAsync(e => e.StudentId == studentId);

            if (enrolledCourseCount >= 3)
            {
                ModelState.AddModelError(string.Empty, $"El estudiante ya tiene inscritas 3 materias.");
                var courses = await _context.Courses.ToListAsync();
                ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");
                ViewBag.StudentId = studentId;
                ViewBag.Student = Student.FirstName + " " + Student.LastName;
                return View();
            }
            // Verificar si el estudiante ya tiene materias inscritas con el mismo profesor
            var existingEnrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId && e.ProfessorId == professor.ProfessorId)
                .ToListAsync();

            if (existingEnrollments.Any())
            {
                ModelState.AddModelError(string.Empty, $"El estudiante ya está inscrito en un curso con el mismo profesor.");
                var courses = await _context.Courses.ToListAsync();
                ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");
                ViewBag.StudentId = studentId;
                ViewBag.Student = Student.FirstName + " " + Student.LastName;
                return View();
            }

            // Crear una nueva inscripción
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                ProfessorId = professor.ProfessorId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            var selectedCourseNames = new List<string> { course.CourseName };
            return View("InscribirMConfirm", selectedCourseNames);
        }

        public async Task<IActionResult> Materias(int? studentId)
        {
            if (studentId == null)
            {
                return NotFound();
            }

            // Obtener inscripciones del estudiante (Enrollment)
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Course.Enrollments)
                .ThenInclude(e => e.Student)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            if (enrollments == null || !enrollments.Any())
            {
                return NotFound();
            }

            // Preparar los datos para mostrar en la vista
            var coursesWithStudents = enrollments.Select(e => e.Course);

            return View(coursesWithStudents);
        }
    }
}
