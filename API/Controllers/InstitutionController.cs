using API.Abstractions;
using API.Implementations.Repository.Entities;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/institutions")]
[TranslateResultToActionResult]
public class InstitutionController : ControllerBase
{
    private readonly IInstitutionDomain _institutionDomain;

    public InstitutionController(IInstitutionDomain institutionDomain)
    {
        _institutionDomain = institutionDomain;
    }

    [HttpGet("{institutionId}")]
    public async Task<Result<Institution>> GetInstitution(int institutionId)
    {
        return await _institutionDomain.GetDetail(institutionId);
    }
}