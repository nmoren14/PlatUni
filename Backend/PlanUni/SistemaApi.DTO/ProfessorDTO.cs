using SistemaApi.Model.Models;
using System;
using System.Collections.Generic;

namespace SistemaApi.DTO;

public class ProfessorDTO
{
    public int ProfessorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int FisrtClass { get; set; }
    public string FisrtClassDescrip { get; set; }
    public int SecondClass { get; set; }
    public string SecondClassDecrip { get; set; }
    

}
