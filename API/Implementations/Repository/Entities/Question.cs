using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class Question
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Column("text")]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    [Column("category")]
    public string Category { get; set; } = string.Empty;
    
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [Column("isDeleted")]
    public bool IsDeleted { get; set; }
}