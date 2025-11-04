using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class SkillsAssessmentRequest
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public List<int> SkillIds { get; set; }
}
