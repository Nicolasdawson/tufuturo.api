using API.Implementations.Repository.Entities;
using API.Models;
using Ardalis.Result;

namespace API.Abstractions;

public interface IStudentDomain
{
    Task<Result<Student>> CreateStudent(StudentRequest request);
}