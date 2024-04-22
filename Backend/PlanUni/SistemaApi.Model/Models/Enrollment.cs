using System;
using System.Collections.Generic;

namespace SistemaApi.Model.Models;

public partial class Enrollment
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int? ProfessorId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Professor? Professor { get; set; }

    public virtual Student Student { get; set; } = null!;
}
