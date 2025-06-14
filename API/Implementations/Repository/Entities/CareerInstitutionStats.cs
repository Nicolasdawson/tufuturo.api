using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerInstitutionStats : GenericEntity
{
    [Required]
    [Column("studyContinuity")]
    public decimal StudyContinuity { get; set; }
    
    [Required]
    [Column("retentionFirstYear")]
    public decimal RetentionFirstYear { get; set; }
    
    [Required]
    [Column("realDuration")]
    public decimal RealDuration { get; set; }
    
    [Required]
    [Column("employabilityFirstYear")]
    public decimal EmployabilityFirstYear { get; set; }
    
    [Required]
    [Column("employabilitySecondYear")]
    public decimal EmployabilitySecondYear { get; set; }
    
    [Required]
    [Column("avarageSalaryFrom")]
    public int AvarageSalaryFrom { get; set; }
    
    [Required]
    [Column("avarageSalaryTo")]
    public int AvarageSalaryTo { get; set; }
    
    [Required]
    [Column("yearOfData")]
    public int YearOfData { get; set; }
        
    [Column("careerInstitutionId")]
    public int CareerInstitutionId { get; set; }
    
    [ForeignKey("careerInstitutionId")]
    public CareerInstitution CareerInstitution { get; set; }
}