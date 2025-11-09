using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class RecommendationRequest
{
    [Required]
    public int RegionId { get; set; }
    
    [Required]
    public List<AnswerRequest> AnswersRiasec { get; set; }
    
}