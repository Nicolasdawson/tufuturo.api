using API.Implementations.Abstractions;
using API.Models;
using Ardalis.Result;
using Entities = API.Implementations.Repository.Entities;
using API.Abstractions;

namespace API.Implementations;

public class SkillsAssessmentDomain : ISkillsAssessmentDomain
{
    private readonly IRepository<Entities.Student> _studentRepository;
    private readonly IRepository<Entities.Skill> _skillRepository;
    private readonly IRepository<Entities.SkillsAssessment> _skillsAssessmentRepository;

    public SkillsAssessmentDomain(
        IRepository<Entities.Student> studentRepository,
        IRepository<Entities.Skill> skillRepository,
        IRepository<Entities.SkillsAssessment> skillsAssessmentRepository)
    {
        _studentRepository = studentRepository;
        _skillRepository = skillRepository;
        _skillsAssessmentRepository = skillsAssessmentRepository;
    }

    public async Task<Result<SkillsAssessment>> CreateSkillsAssessment(SkillsAssessmentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId);

        if (student is null)
        {
            return Result<SkillsAssessment>.NotFound("Student not found");
        }

        var skills = await _skillRepository.GetAllAsync();
        var selectedSkills = skills.Where(s => request.SkillIds.Contains(s.Id)).ToList();

        var skillsAssessment = new Entities.SkillsAssessment
        {
            StudentId = request.StudentId,
            Skills = selectedSkills
        };

        await _skillsAssessmentRepository.AddAsync(skillsAssessment);

        return Result<SkillsAssessment>.Success(new SkillsAssessment
        {
            Id = skillsAssessment.Id,
            StudentId = skillsAssessment.StudentId,
            Skills = selectedSkills.Select(s => new Skill { Id = s.Id, Name = s.Name, Type = s.Type }).ToList()
        });
    }
}
