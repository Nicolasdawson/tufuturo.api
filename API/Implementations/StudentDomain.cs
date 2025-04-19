using API.Abstractions;
using API.Implementations.Repository.Entities;
using API.Models;
using Ardalis.Result;

namespace API.Implementations;

public class StudentDomain : IStudentDomain
{
    private readonly IStudentRepository _studentRepository;

    public StudentDomain(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Result<Student>> CreateStudent(StudentRequest request)
    {
        var emailAlreadyExists = await _studentRepository.CheckEmail(request.Email);

        if (emailAlreadyExists)
            return Result<Student>.Conflict("Email already exists");
        
        return Result<Student>.Created(await _studentRepository.AddAsync(new Student
        {
            Name = request.Name,
            Email = request.Email,
            Answers = request.Answers
        }));
    }
}