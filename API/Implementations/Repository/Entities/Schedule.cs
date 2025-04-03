using System.ComponentModel.DataAnnotations;

namespace API.Implementations.Repository.Entities;
public class Schedule
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}