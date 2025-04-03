using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class Statistics
{
    [Key]
    public int Id { get; set; }
    
    public int CareerId { get; set; }
    
    [ForeignKey("CareerId")]
    public Career? Career { get; set; }
    
    [Required]
    public int Enrollment { get; set; }
    
    [Required]
    public decimal GraduationRate { get; set; }
    
    [Required]
    public decimal DropoutRate { get; set; }
}