using API.Implementations;
using API.Models;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/careers")]
[TranslateResultToActionResult]
public class CareersController : ControllerBase
{
    private readonly ICareerDomain _careerDomain;

    public CareersController(ICareerDomain careerDomain)
    {
        _careerDomain = careerDomain;
    }

    [HttpGet]
    public async Task<PagedResult<List<Models.Career>>> GetCareers([FromQuery] CareerParams queryparams)
    {
        return await _careerDomain.GetCareers(queryparams);
    }

    [HttpGet("institution/{institutionId}")]
    public async Task<Result<List<Models.CareerInstitution>>> GetCareersInstitution(int institutionId)
    {
        return await _careerDomain.GetCareersInstitution(institutionId);
    }

    [HttpGet("institution/{institutionId}/campus/{campusId}")]
    public async Task<Result<List<Models.CareerCampus>>> GetCareersCampus(int institutionId, int campusId)
    {
        return await _careerDomain.GetCareersCampus(institutionId, campusId);
    }
}