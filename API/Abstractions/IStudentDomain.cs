using API.Implementations.Repository.Entities;
using API.Models;

namespace API.Abstractions;

public interface IStudentDomain
{
    Task<Student?> CreateStudent(StudentRequest request);
}