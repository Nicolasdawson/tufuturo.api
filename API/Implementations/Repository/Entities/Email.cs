using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Implementations.Repository.Entities;

public class Email : GenericEntity
{
    [Required]
    [MaxLength(255)]
    [Column("type")]
    public string Type { get; set; } = string.Empty;
    
    [Required]
    [Column("deliverydate")]
    public DateOnly DeliveryDate { get; set; }
    
    [Required]
    [Column("isdelivered")]
    public bool IsDelivered { get; set; }
    
    [Column("studentid")]
    public int StudentId { get; set; }
    
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }
}