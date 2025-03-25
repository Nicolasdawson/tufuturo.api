using API.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IAssessmentService _assessmentService;

    public QuestionsController(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuestions()
    {
        var questions = await _assessmentService.GetQuestions();
        return Ok(questions);
    }
}