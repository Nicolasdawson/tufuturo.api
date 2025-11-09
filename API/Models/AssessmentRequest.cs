using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class AssessmentRequest
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public List<AnswerRequest> AnswersRiasec { get; set; }
    
}