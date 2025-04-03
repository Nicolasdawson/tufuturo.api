using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class WeightingStudent
{
    [Key]
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string TestType { get; set; } = string.Empty;
    
    [Required]
    public decimal Weight { get; set; }
}