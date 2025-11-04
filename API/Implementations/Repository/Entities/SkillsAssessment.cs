using System.Collections.Generic;

namespace API.Implementations.Repository.Entities;

public class SkillsAssessment : GenericEntity
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public List<Skill> Skills { get; set; }
}
