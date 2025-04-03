using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;
public class Career
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public int InstitutionId { get; set; }
    public int KnowledgeAreaId { get; set; }
    public int ScheduleId { get; set; }
    
    [ForeignKey("InstitutionId")]
    public Institution? Institution { get; set; }
    
    [ForeignKey("KnowledgeAreaId")]
    public KnowledgeArea? KnowledgeArea { get; set; }
    
    [ForeignKey("ScheduleId")]
    public Schedule? Schedule { get; set; }
}