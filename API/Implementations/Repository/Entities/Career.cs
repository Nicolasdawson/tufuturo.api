using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;
public class Career
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("institutionId")]
    public int InstitutionId { get; set; }
    
    [Column("knowledgeAreaId")]
    public int KnowledgeAreaId { get; set; }
    
    [Column("scheduleId")]
    public int ScheduleId { get; set; }
    
    [ForeignKey("InstitutionId")]
    public Institution? Institution { get; set; }
    
    [ForeignKey("KnowledgeAreaId")]
    public KnowledgeArea? KnowledgeArea { get; set; }
    
    [ForeignKey("ScheduleId")]
    public Schedule? Schedule { get; set; }
}