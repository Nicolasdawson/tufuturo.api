using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class WeightingCareer
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
    
    [Column("careerId")]
    public int CareerId { get; set; }
    
    [ForeignKey("CareerId")]
    public Career? Career { get; set; }
}