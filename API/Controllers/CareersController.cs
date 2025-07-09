using System.Net;
using API.Implementations;
using API.Models;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/careers")]
public class CareersController : ControllerBase
{
    private readonly ICareerDomain _careerDomain;

    public CareersController(ICareerDomain careerDomain)
    {
        _careerDomain = careerDomain;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<List<Models.Career>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetParentCareers([FromQuery] CareerParams queryparams)
    {
        var result = await _careerDomain.GetParentCareers(queryparams);
        return Ok(result);
    }
    
    [HttpGet("{careerId}")]
    public Result<List<Models.CareerInstitution>> GetCareer(int careerId)
    {
        return _careerDomain.GetCareerByGenericCareer(careerId);
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