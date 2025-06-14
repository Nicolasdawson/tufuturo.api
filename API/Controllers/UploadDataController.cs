using API.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadDataController : ControllerBase
{
    private readonly IUploadDataDomain _uploadDataDomain;

    public UploadDataController(IUploadDataDomain uploadDataDomain)
    {
        _uploadDataDomain = uploadDataDomain;
    }
    
    [HttpPost("institutions")]
    public async Task PostInstitutions([FromForm] FileStream file)
    {
       await _uploadDataDomain.UploadInstitutions(file);
    }
    
    [HttpPost("careers")]
    public async Task PostCareers([FromForm] FileStream file)
    {
        await _uploadDataDomain.UploadGenericsCareers(file);
    }
    
    [HttpPost("institutions/careers")]
    public async Task PostCareersInstitutions([FromForm] FileStream file)
    {
        await _uploadDataDomain.UploadCareersInstitution(file);
    }
    
    [HttpPost("institutions/campus")]
    public async Task PostInstitutionsCampus([FromForm] FileStream file)
    {
        await _uploadDataDomain.UploadInstitutionCampus(file);
    }
    
    [HttpPost("institutions/campus/careers")]
    public async Task CareersCampus([FromForm] FileStream file)
    {
        await _uploadDataDomain.UploadCareersCampus(file);
    }
}