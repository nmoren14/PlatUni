using SistemaApi.DTO;
using SistemaApi.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaApi.BLL.Servicios.Contrato
{
    public interface IProfessorService
    {
        Task<List<ProfessorDTO>> GetAllProfessorsAsync();
        Task<ProfessorDTO> GetProfessorByIdAsync(int professorId);
        Task<ProfessorDTO> CreateProfessorAsync(ProfessorDTO professorDTO);
        Task<ProfessorDTO> UpdateProfessorAsync(int professorId, Professor professor);
        Task<bool> DeleteProfessorAsync(int professorId);
    }
}
