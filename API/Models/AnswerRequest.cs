using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class AnswerRequest
{
    [Required]
    [Range(1, 30)]
    public int QuestionId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Score { get; set; }
}