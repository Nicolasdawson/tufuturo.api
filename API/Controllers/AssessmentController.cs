using API.Abstractions;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssessmentController : ControllerBase
{
    private readonly IAssessmentService _assessmentService;

    public AssessmentController(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpPost()]
    public async Task<IActionResult> SubmitAssessment([FromBody] SubmitAssessmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assessmentService.CalculateResult(
            request.UserId,
            request.Answers.Select(a => new Answer
            {
                QuestionId = a.QuestionId,
                Score = a.Score
            }).ToList());

        var careers = await _assessmentService.GetCareerSuggestions(result);

        return Ok(new AssessmentResponse
        {
            Result = result,
            CareerSuggestions = careers
        });
    }
}