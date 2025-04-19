using API.Abstractions;
using API.Implementations.Repository.Entities;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[TranslateResultToActionResult]
public class StudentsController : ControllerBase
{
    private readonly IStudentDomain _studentDomain;

    public StudentsController(IStudentDomain studentDomain)
    {
        _studentDomain = studentDomain;
    }
    
    [HttpPost]
    public async Task<Result<Student>> CreateStudent([FromBody] StudentRequest request)
    {
        return await _studentDomain.CreateStudent(request);
    }
}