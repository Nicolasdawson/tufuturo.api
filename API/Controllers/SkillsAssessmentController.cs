using API.Implementations.Abstractions;
using API.Models;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/skills-assessments")]
[TranslateResultToActionResult]
public class SkillsAssessmentController : ControllerBase
{
    private readonly ISkillsAssessmentDomain _skillsAssessmentDomain;

    public SkillsAssessmentController(ISkillsAssessmentDomain skillsAssessmentDomain)
    {
        _skillsAssessmentDomain = skillsAssessmentDomain;
    }

    [HttpPost]
    public async Task<Result<SkillsAssessment>> SubmitSkillsAssessment([FromBody] SkillsAssessmentRequest request)
    {
        return await _skillsAssessmentDomain.CreateSkillsAssessment(request);
    }
}
