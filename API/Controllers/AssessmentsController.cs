using API.Abstractions;
using API.Models;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/assessments")]
[TranslateResultToActionResult]
public class AssessmentsController : ControllerBase
{
    private readonly IAssessmentDomain _assessmentDomain;

    public AssessmentsController(IAssessmentDomain assessmentDomain)
    {
        _assessmentDomain = assessmentDomain;
    }

    [HttpPost]
    public async Task<Result> SubmitAssessment([FromBody] AssessmentRequest request)
    {
        return await _assessmentDomain.CreateAssessment(request);
    }
}