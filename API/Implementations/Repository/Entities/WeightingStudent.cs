using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class WeightingStudent
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("testType")]
    public string TestType { get; set; } = string.Empty;
    
    [Required]
    [Column("weight")]
    public decimal Weight { get; set; }
    
    [Column("studentId")]
    public int StudentId { get; set; }
    
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }
}