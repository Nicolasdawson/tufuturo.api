namespace API.Models;

public class SkillsAssessment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public List<Skill> Skills { get; set; }
}
