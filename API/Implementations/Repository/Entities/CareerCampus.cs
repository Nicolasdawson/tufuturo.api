using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerCampus : GenericEntity
{
    [Column("name")]
    public required string Name { get; set; }
    
    [Column("careerInstitution")]
    public int CareerInstitutionId { get; set; }
    
    [ForeignKey("careerInstitution")]
    public CareerInstitution CareerInstitution { get; set; }
    
    [Column("scheduleId")]
    public int ScheduleId { get; set; }
    
    [ForeignKey("scheduleId")]
    public Schedule Schedule { get; set; }
    
    [Column("institutionCampusId")]
    public int InstitutionCampusId { get; set; }
    
    [ForeignKey("institutionCampusId")]
    public InstitutionCampus InstitutionCampus { get; set; }
    
    public virtual ICollection<CareerCampusStats> CareerCampusStats { get; set; } = new List<CareerCampusStats>();
}