using API.Models;
using Ardalis.Result;

namespace API.Implementations.Abstractions;

public interface ISkillsAssessmentDomain
{
    Task<Result<SkillsAssessment>> CreateSkillsAssessment(SkillsAssessmentRequest request);
}
