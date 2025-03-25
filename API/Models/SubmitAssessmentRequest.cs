using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class SubmitAssessmentRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MinLength(10)]
    public List<AnswerRequest> Answers { get; set; } = new();
}