using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class WeightingCareer
{
    [Key]
    public int Id { get; set; }
    
    public int CareerId { get; set; }
    
    [ForeignKey("CareerId")]
    public Career? Career { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string TestType { get; set; } = string.Empty;
    
    [Required]
    public decimal Weight { get; set; }
}