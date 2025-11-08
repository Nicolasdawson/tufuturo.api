using API.Models;
using Ardalis.Result;

namespace API.Abstractions;

public interface IRecommendationDomain
{
    Task<Result<List<RecommendedCareer>>> GetRecommendations(AssessmentRequest request);
}
