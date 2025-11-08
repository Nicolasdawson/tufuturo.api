using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;
public class Career : GenericEntity
{
    [MaxLength(255)]
    [Column("name")]
    public required string Name { get; set; }
    
    [Column("knowledgeAreaId")]
    public int KnowledgeAreaId { get; set; }
    
    [ForeignKey("KnowledgeAreaId")]
    public KnowledgeArea KnowledgeArea { get; set; }
    
    public virtual ICollection<CareerInstitution> CareerInstitutions { get; set; } = new List<CareerInstitution>();
}