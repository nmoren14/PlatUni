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
    public class ProfessorService : IProfessorService
    {
        private readonly PlatUniContext _context;
        private readonly IMapper _mapper;
        public ProfessorService(PlatUniContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProfessorDTO>> GetAllProfessorsAsync()
        {
            var professors = await _context.Professors.ToListAsync();

            // Consultar nombres de materias para FisrtClassDescrip y SecondClassDecrip
            var coursesIds = professors.Select(p => p.FisrtClass).Concat(professors.Select(p => p.SecondClass)).Distinct().ToList();
            var coursesDict = await _context.Courses.Where(c => coursesIds.Contains(c.CourseId)).ToDictionaryAsync(c => c.CourseId);

            // Mapear a ProfessorDTO con descripciones de materias
            var professorDTOs = professors.Select(p => new ProfessorDTO
            {
                ProfessorId = p.ProfessorId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                FisrtClass = p.FisrtClass,
                FisrtClassDescrip = coursesDict.ContainsKey(p.FisrtClass) ? coursesDict[p.FisrtClass].CourseName : null,
                SecondClass = p.SecondClass,
                SecondClassDecrip = coursesDict.ContainsKey(p.SecondClass) ? coursesDict[p.SecondClass].CourseName : null
            }).ToList();

            return professorDTOs;
        }

        public async Task<ProfessorDTO> GetProfessorByIdAsync(int professorId)
        {
            var professor = await _context.Professors.FindAsync(professorId);
            if (professor == null)
            {
                return null; // Profesor no encontrado
            }

            // Consultar nombres de materias para FisrtClassDescrip y SecondClassDecrip
            var coursesIds = new List<int> { professor.FisrtClass, professor.SecondClass };
            var coursesDict = await _context.Courses.Where(c => coursesIds.Contains(c.CourseId)).ToDictionaryAsync(c => c.CourseId);

            // Mapear a ProfessorDTO con descripciones de materias
            var professorDTO = new ProfessorDTO
            {
                ProfessorId = professor.ProfessorId,
                FirstName = professor.FirstName,
                LastName = professor.LastName,
                FisrtClass = professor.FisrtClass,
                FisrtClassDescrip = coursesDict.ContainsKey(professor.FisrtClass) ? coursesDict[professor.FisrtClass].CourseName : null,
                SecondClass = professor.SecondClass,
                SecondClassDecrip = coursesDict.ContainsKey(professor.SecondClass) ? coursesDict[professor.SecondClass].CourseName : null
            };

            return professorDTO;
        }
        public async Task<ProfessorDTO> CreateProfessorAsync(ProfessorDTO professorDTO)
        {
            // Mapear desde ProfessorDTO a Professor utilizando AutoMapper
            var professor = _mapper.Map<Professor>(professorDTO);

            // Agregar el nuevo profesor a la base de datos
            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();

            // Mapear de nuevo a ProfessorDTO para devolver como resultado
            var createdProfessorDTO = _mapper.Map<ProfessorDTO>(professor);
            return createdProfessorDTO;
        }

        public async Task<ProfessorDTO> UpdateProfessorAsync(int professorId, Professor professor)
        {
            if (professorId != professor.ProfessorId)
            {
                return null; // Id no coincide
            }

            _context.Entry(professor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Consultar nombres de materias para FisrtClassDescrip y SecondClassDecrip después de la actualización
                var coursesIds = new List<int> { professor.FisrtClass, professor.SecondClass };
                var coursesDict = await _context.Courses.Where(c => coursesIds.Contains(c.CourseId)).ToDictionaryAsync(c => c.CourseId);

                // Mapear a ProfessorDTO con descripciones de materias actualizadas
                var professorDTO = new ProfessorDTO
                {
                    ProfessorId = professor.ProfessorId,
                    FirstName = professor.FirstName,
                    LastName = professor.LastName,
                    FisrtClass = professor.FisrtClass,
                    FisrtClassDescrip = coursesDict.ContainsKey(professor.FisrtClass) ? coursesDict[professor.FisrtClass].CourseName : null,
                    SecondClass = professor.SecondClass,
                    SecondClassDecrip = coursesDict.ContainsKey(professor.SecondClass) ? coursesDict[professor.SecondClass].CourseName : null
                };

                return professorDTO;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessorExists(professorId))
                {
                    return null; // Profesor no encontrado
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProfessorAsync(int professorId)
        {
            var professor = await _context.Professors.FindAsync(professorId);
            if (professor == null)
            {
                return false; // Profesor no encontrado
            }

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professors.Any(e => e.ProfessorId == id);
        }
    }
}