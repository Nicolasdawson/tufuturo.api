using API.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
// [ApiExplorerSettings(IgnoreApi = true)]
[Route("api/upload-data")]
public class UploadDataController : ControllerBase
{
    private readonly IUploadDataDomain _uploadDataDomain;

    public UploadDataController(IUploadDataDomain uploadDataDomain)
    {
        _uploadDataDomain = uploadDataDomain;
    }
    
    [HttpPost("institutions")]
    [Consumes("multipart/form-data")]
    public async Task PostInstitutions(IFormFile file)
    {
        using var stream = file.OpenReadStream();
       await _uploadDataDomain.UploadInstitutions(stream);
    }
    
    [HttpPost("careers")]
    public async Task PostCareers( FileStream file)
    {
        await _uploadDataDomain.UploadGenericsCareers(file);
    }
    
    [HttpPost("institutions/careers")]
    public async Task PostCareersInstitutions( FileStream file)
    {
        await _uploadDataDomain.UploadCareersInstitution(file);
    }
    
    [HttpPost("institutions/campus")]
    public async Task PostInstitutionsCampus( FileStream file)
    {
        await _uploadDataDomain.UploadInstitutionCampus(file);
    }
    
    [HttpPost("institutions/campus/careers")]
    public async Task CareersCampus( FileStream file)
    {
        await _uploadDataDomain.UploadCareersCampus(file);
    }
}