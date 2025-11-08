using API.Abstractions;
using API.Models;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/recommendations")]
[TranslateResultToActionResult]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationDomain _recommendationDomain;

    public RecommendationsController(IRecommendationDomain recommendationDomain)
    {
        _recommendationDomain = recommendationDomain;
    }

    [HttpPost]
    public async Task<Result<List<RecommendedCareer>>> GetRecommendations([FromBody] AssessmentRequest request)
    {
        return await _recommendationDomain.GetRecommendations(request);
    }
}
