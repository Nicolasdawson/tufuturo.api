using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class InstitutionCampus : GenericEntity
{
    [Column("name")]
    public required string Name { get; set; }
    
    [Column("regionId")]
    public int RegionId { get; set; }
    
    [ForeignKey("RegionId")]
    public Region Region { get; set; }
    
    [Column("institutionId")]
    public int InstitutionId { get; set; }
    
    [ForeignKey("InstitutionId")]
    public Institution Institution { get; set; }
}