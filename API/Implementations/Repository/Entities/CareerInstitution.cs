using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class CareerInstitution : GenericEntity
{
    [Column("name")]
    public required string Name { get; set; }
    
    [Column("carrerId")]
    public int CarrerId { get; set; }
    
    [ForeignKey("carrerId")]
    public Career Career { get; set; }
    
    [Column("institutionId")]
    public int InstitutionId { get; set; }
    
    [ForeignKey("institutionId")]
    public Institution Institution { get; set; }
    public virtual ICollection<CareerInstitutionStats> CareerInstitutionStats { get; set; } = new List<CareerInstitutionStats>();
    public virtual ICollection<CareerCampus> CareerCampuses { get; set; } = new List<CareerCampus>();
}