using API.Abstractions;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api")]
public class CatalogsController : ControllerBase
{
    private readonly ICatalogsDomain _catalogsDomain;

    public CatalogsController(ICatalogsDomain catalogsDomain)
    {
        _catalogsDomain = catalogsDomain;
    }
    
    [HttpGet]
    [Route("regions")]
    public async Task<IActionResult> GetRegions()
    {
        var regions = await _catalogsDomain.GetRegions();
        return Ok(regions);
    }
    
    [HttpGet]
    [Route("acreditation-types")]
    public async Task<IActionResult> GetAcreditationTypes()
    {
        var acreditationTypes = await _catalogsDomain.GetAcreditationTypes();
        return Ok(acreditationTypes);
    }
    
    [HttpGet]
    [Route("institution-types")]
    public async Task<IActionResult> GetInstitutionTypes()
    {
        var institutionTypes = await _catalogsDomain.GetInstitutionTypes();
        return Ok(institutionTypes);
    }
    
    [HttpGet]
    [Route("knowledge-areas")]
    public async Task<IActionResult> GetKnowledgeAreas()
    {
        var knowledgeAreas = await _catalogsDomain.GetKnowledgeAreas();
        return Ok(knowledgeAreas);
    }
    
    [HttpGet]
    [Route("schedules")]
    public async Task<IActionResult> GetSchedules()
    {
        var schedules = await _catalogsDomain.GetSchedules();
        return Ok(schedules);
    }
}