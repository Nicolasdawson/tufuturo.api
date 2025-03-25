using API.Models;

namespace API.Abstractions;

public interface IResultRepository
{
    Task SaveResultAsync(AssessmentResult result);
    
    Task<AssessmentResult?> GetResultByUserIdAsync(Guid userId);
}