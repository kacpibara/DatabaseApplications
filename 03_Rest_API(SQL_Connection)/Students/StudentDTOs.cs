using System.ComponentModel.DataAnnotations;

namespace Task3Application.Students;

public class CreateStudentDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

