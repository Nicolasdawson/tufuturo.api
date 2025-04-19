using API.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsDomain _questionsDomain;

    public QuestionsController(IQuestionsDomain questionsDomain)
    {
        _questionsDomain = questionsDomain;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuestions()
    {
        var questions = await _questionsDomain.GetAllQuestions();
        return Ok(questions);
    }
}