using API.Abstractions;
using API.Implementations.Repository.Entities;
using API.Models;

namespace API.Implementations;

public class StudentDomain : IStudentDomain
{
    private readonly IStudentRepository _studentRepository;

    public StudentDomain(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student?> CreateStudent(StudentRequest request)
    {
        try
        {
            return await _studentRepository.AddAsync(new Student
            {
                Name = request.Name,
                Email = request.Email,
                Answers = request.Answers
            });
        }
        catch (Exception exception)
        {
            return null;
        }
    }
}