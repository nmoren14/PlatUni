using SistemaApi.Model.Models;
using System;
using System.Collections.Generic;

namespace SistemaApi.DTO;

public class StudentDTO
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public int EnrollmentCount { get; set; }
}
