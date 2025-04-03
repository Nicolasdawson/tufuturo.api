using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface IStudentRepository : IRepository<Student>
{
    Task<bool> CheckEmail(string email);
}