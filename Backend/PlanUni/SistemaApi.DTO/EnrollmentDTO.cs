using System;
using System.Collections.Generic;

namespace SistemaApi.DTO;

public class EnrollmentDTO
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int? ProfessorId { get; set; }

}
