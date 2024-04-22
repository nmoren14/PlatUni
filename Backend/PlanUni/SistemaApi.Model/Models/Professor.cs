using System;
using System.Collections.Generic;

namespace SistemaApi.Model.Models;

public partial class Professor
{
    public int ProfessorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int FisrtClass { get; set; }

    public int SecondClass { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Course FisrtClassNavigation { get; set; } = null!;

    public virtual Course SecondClassNavigation { get; set; } = null!;
}
