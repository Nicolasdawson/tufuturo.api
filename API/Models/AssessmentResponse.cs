namespace API.Models;

public class AssessmentResponse
{
    public AssessmentResult Result { get; set; } = null!;
    public List<CareerSuggestion> CareerSuggestions { get; set; } = new();
}