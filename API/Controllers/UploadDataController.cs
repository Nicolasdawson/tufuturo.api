using API.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
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
    public async Task PostCareers(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        await _uploadDataDomain.UploadGenericsCareers(stream);
    }
    
    [HttpPost("institutions/careers")]
    public async Task PostCareersInstitutions(IFormFile file)
    {
        // TODO: **AUNQUE** hay que cambiar la tabla de detalle
        // estaba ocupando Buscador_Empleabilidad_e_Ingresos_2024_2025_SIES.xlsx
        // y Buscador_Estadisticas_por_carrera_2024_2025_SIES.xlsx 
        // tiene muchos mejores datos
        using var stream = file.OpenReadStream();
        await _uploadDataDomain.UploadCareersInstitution(stream);
    }
    
    [HttpPost("institutions/campus")]
    public async Task PostInstitutionsCampus(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        await _uploadDataDomain.UploadInstitutionCampus(stream);
    }
    
    [HttpPost("institutions/campus/careers")]
    public async Task CareersCampus(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        await _uploadDataDomain.UploadCareersCampus(stream);
    }
}