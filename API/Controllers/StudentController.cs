using API.Abstractions;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentDomain _studentDomain;

    public StudentsController(IStudentDomain studentDomain)
    {
        _studentDomain = studentDomain;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostStudent([FromBody] StudentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _studentDomain.CreateStudent(request);

        if (result is not null)
        {
            return Ok(result);
        }
        
        return BadRequest();
    }
}