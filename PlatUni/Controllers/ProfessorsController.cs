using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlatUni.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PlatUni.Controllers
{
    public class ProfessorsController : Controller
    {
        private readonly PlatUniContext _context;

        public ProfessorsController(PlatUniContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var professors = await _context.Professors
        .Include(p => p.FisrtClassNavigation) 
        .Include(p => p.SecondClassNavigation) 
        .ToListAsync();

            return View(professors);
        }

        public IActionResult Create()
        {
            var courses = _context.Courses.ToList();

            ViewBag.Courses = new SelectList(courses, "CourseId", "CourseName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfessorId,FirstName,LastName,FisrtClass,SecondClass")] Professor professor)
        {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            var courses = await _context.Courses.ToListAsync();
            ViewBag.Courses = courses.Select(c => new SelectListItem
            {
                Value = c.CourseId.ToString(), // Asignar el ID del curso como Value
                Text = c.CourseName // Asignar el nombre del curso como Text
            }).ToList();

            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfessorId,FirstName,LastName,FisrtClass,SecondClass")] Professor professor)
        {
            if (id != professor.ProfessorId)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(professor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.ProfessorId))
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professors
                .Include(p => p.FisrtClassNavigation)
                .Include(p => p.SecondClassNavigation)
                .FirstOrDefaultAsync(m => m.ProfessorId == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professors.Any(e => e.ProfessorId == id);
        }
    }
}
