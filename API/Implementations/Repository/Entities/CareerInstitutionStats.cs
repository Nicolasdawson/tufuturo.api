using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerInstitutionStats : GenericEntity
{
    [Required]
    [Column("studycontinuity")]
    public decimal StudyContinuity { get; set; }
    
    [Required]
    [Column("retentionfirstyear")]
    public decimal RetentionFirstYear { get; set; }
    
    [Required]
    [Column("realduration")]
    public decimal RealDuration { get; set; }
    
    [Required]
    [Column("employabilityfirstyear")]
    public decimal EmployabilityFirstYear { get; set; }
    
    [Required]
    [Column("employabilitysecondyear")]
    public decimal EmployabilitySecondYear { get; set; }
    
    [Required]
    [Column("avaragesalaryfrom")]
    public int AvarageSalaryFrom { get; set; }
    
    [Required]
    [Column("avaragesalaryto")]
    public int AvarageSalaryTo { get; set; }
    
    [Required]
    [Column("yearofdata")]
    public int YearOfData { get; set; }
        
    [Column("careerinstitutionid")]
    public int CareerInstitutionId { get; set; }
    
    [ForeignKey("CareerInstitutionId")]
    public CareerInstitution CareerInstitution { get; set; }
}