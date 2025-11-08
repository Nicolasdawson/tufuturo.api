using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerCampus : GenericEntity
{
    [Column("name")]
    public string Name { get; set; }
    
    [Column("careerInstitutionId")]
    public int CareerInstitutionId { get; set; }
    
    [ForeignKey("CareerInstitutionId")]
    public CareerInstitution CareerInstitution { get; set; }
    
    [Column("scheduleId")]
    public int ScheduleId { get; set; }
    
    [ForeignKey("ScheduleId")]
    public Schedule Schedule { get; set; }
    
    [Column("institutionCampusId")]
    public int InstitutionCampusId { get; set; }
    
    [ForeignKey("InstitutionCampusId")]
    public InstitutionCampus InstitutionCampus { get; set; }
    
    public virtual ICollection<CareerCampusStats> CareerCampusStats { get; set; } = new List<CareerCampusStats>();
}