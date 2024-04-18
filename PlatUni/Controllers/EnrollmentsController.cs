using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatUni;
using PlatUni.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PlatUni.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly PlatUniContext _context;

        public EnrollmentsController(PlatUniContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enrollments.ToListAsync());
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,CourseId,ProfessorId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? studentId, int? courseId)
        {
            if (studentId == null || courseId == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseId == courseId);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseId == courseId);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}