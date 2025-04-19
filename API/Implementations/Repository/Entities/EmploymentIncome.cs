using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class EmploymentIncome
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("employmentRate")]
    public decimal EmploymentRate { get; set; }
    
    [Required]
    [Column("averageIncome")]
    public int AverageIncome { get; set; }
    
    [Required]
    [Column("medianIncome")]
    public int MedianIncome { get; set; }
    
    [Column("careerId")]
    public int CareerId { get; set; }
    
    [ForeignKey("CareerId")]
    public Career? Career { get; set; }
}