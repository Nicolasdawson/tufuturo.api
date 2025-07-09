using API.Abstractions;
using API.Implementations.Repository.Entities;
using API.Models;
using Ardalis.Result;
using Entities = API.Implementations.Repository.Entities;

namespace API.Implementations;

public class StudentDomain : IStudentDomain
{
    private readonly IRepository<Entities.Student> _studentRepository;

    public StudentDomain(IRepository<Entities.Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<Student>> CreateStudent(StudentRequest request)
    {
        var emailAlreadyExists = await _studentRepository.AnyAsync(x => x.Email == request.Email);

        if (emailAlreadyExists)
            return Result<Student>.Conflict("Email already exists");
        
        return Result<Student>.Created(await _studentRepository.AddAsync(new Student
        {
            Name = request.Name,
            Email = request.Email
        }));
    }
}