using System;
using System.Collections.Generic;

namespace SistemaApi.Model.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int Credits { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Professor> ProfessorFisrtClassNavigations { get; set; } = new List<Professor>();

    public virtual ICollection<Professor> ProfessorSecondClassNavigations { get; set; } = new List<Professor>();
}
