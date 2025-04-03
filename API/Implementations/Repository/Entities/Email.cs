using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class Email
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Type { get; set; } = string.Empty;
    
    public int StudentId { get; set; }
    
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }
    
    [Required]
    public DateOnly DeliveryDate { get; set; }
    
    [Required]
    public bool IsDelivered { get; set; }
}