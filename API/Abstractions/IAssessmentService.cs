using API.Models;

namespace API.Abstractions;

public interface IAssessmentService
{
    Task<List<Question>> GetQuestions();
    
    Task<AssessmentResult> CalculateResult(Guid userId, List<Answer> answers);
    
    Task<List<CareerSuggestion>> GetCareerSuggestions(AssessmentResult result);
}