using System.ComponentModel.DataAnnotations;

namespace API.Implementations.Repository.Entities;
public class KnowledgeArea
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
}