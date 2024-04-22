using Microsoft.AspNetCore.Mvc;
using SistemaApi.BLL.Servicios.Contrato;
using SistemaApi.DTO;
using SistemaApi.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("[controller]")]
[ApiController]
public class ProfessorsController : ControllerBase
{
    private readonly IProfessorService _professorService;

    public ProfessorsController(IProfessorService professorService)
    {
        _professorService = professorService;
    }

    [HttpGet("GetAllProfessors")]
    public async Task<ActionResult<List<Professor>>> GetAllProfessors()
    {
        var professors = await _professorService.GetAllProfessorsAsync();
        return Ok(professors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfessorDTO>> GetProfessorById(int id)
    {
        var professorDTO = await _professorService.GetProfessorByIdAsync(id);
        if (professorDTO == null)
        {
            return NotFound(); // Retorna 404 si el profesor no se encuentra
        }
        return professorDTO;
    }

    // Ejemplo de método para crear un nuevo profesor
    [HttpPost("CreateProfessor")]
    public async Task<ActionResult<ProfessorDTO>> CreateProfessor(ProfessorDTO professorDTO)
    {
        var createdProfessorDTO = await _professorService.CreateProfessorAsync(professorDTO);
        return CreatedAtAction(nameof(GetProfessorById), new { id = createdProfessorDTO.ProfessorId }, createdProfessorDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfessor(int id, Professor professor)
    {
        var updatedProfessor = await _professorService.UpdateProfessorAsync(id, professor);
        if (updatedProfessor == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfessor(int id)
    {
        var result = await _professorService.DeleteProfessorAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
