using System.Net;
using API.Abstractions;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsDomain _questionsDomain;

    public QuestionsController(IQuestionsDomain questionsDomain)
    {
        _questionsDomain = questionsDomain;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Question>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetQuestions()
    {
        var questions = await _questionsDomain.GetAllQuestions();
        return Ok(questions);
    }
}