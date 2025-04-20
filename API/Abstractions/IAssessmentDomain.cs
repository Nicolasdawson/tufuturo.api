using API.Models;
using Ardalis.Result;

namespace API.Abstractions;

public interface IAssessmentDomain
{
    Task<Result> CreateAssessment(AssessmentRequest request);
}
