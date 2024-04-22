using SistemaApi.Model.Models;
using System;
using System.Collections.Generic;

namespace SistemaApi.DTO;

public class CourseDTO
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int Credits { get; set; }
}
