using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;
public class Institution : GenericEntity
{
    [MaxLength(255)]
    [Column("name")]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    [Column("code")]
    public required string Code { get; set; }
    
    [Column("institutiontypeid")]
    public int InstitutionTypeId { get; set; }
    
    [ForeignKey("InstitutionTypeId")]
    public InstitutionType InstitutionType { get; set; }
    public virtual ICollection<InstitutionDetails> InstitutionDetails { get; set; } = new List<InstitutionDetails>();
    public virtual ICollection<InstitutionCampus> InstitutionCampuses { get; set; } = new List<InstitutionCampus>();
}