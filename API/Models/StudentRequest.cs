using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class StudentRequest
{
    [Required] 
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}