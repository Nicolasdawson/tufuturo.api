using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class EmploymentIncome
{
    [Key]
    public int Id { get; set; }
    
    public int CareerId { get; set; }
    
    [ForeignKey("CareerId")]
    public Career? Career { get; set; }
    
    [Required]
    public decimal EmploymentRate { get; set; }
    
    [Required]
    public int AverageIncome { get; set; }
    
    [Required]
    public int MedianIncome { get; set; }
}