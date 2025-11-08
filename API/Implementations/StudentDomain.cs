using API.Abstractions;
using API.Implementations.Repository.Entities;
using API.Models;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using Entities = API.Implementations.Repository.Entities;

namespace API.Implementations;

public class StudentDomain : IStudentDomain
{
    private readonly IRepository<Entities.Student> _studentRepository;
    private readonly ILogger<StudentDomain> _logger;

    public StudentDomain(IRepository<Entities.Student> studentRepository, ILogger<StudentDomain> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Result<Student>> CreateStudent(StudentRequest request)
    {
        var emailAlreadyExists = await _studentRepository.AnyAsync(x => x.Email == request.Email);

        if (emailAlreadyExists)
        {
            return Result<Student>.Conflict("Email already exists");
        }
        
        var student = await _studentRepository.AddAsync(new Student
        {
            Name = request.Name,
            Email = request.Email
        });
        
        return Result<Student>.Created(student);
    }
}