using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class Statistics
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("enrollment")]
    public int Enrollment { get; set; }
    
    [Required]
    [Column("graduationRate")]
    public decimal GraduationRate { get; set; }
    
    [Required]
    [Column("dropoutRate")]
    public decimal DropoutRate { get; set; }
    
    [Column("careerId")]
    public int CareerId { get; set; }
    
    [ForeignKey("CareerId")]
    public Career? Career { get; set; }
}