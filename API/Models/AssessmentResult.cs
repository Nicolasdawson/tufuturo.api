namespace API.Models;

public class AssessmentResult
{
    public Guid UserId { get; set; }
    
    public DateTime CompletedAt { get; set; }
    public Dictionary<RiasecCategory, int> Scores { get; set; } = new();
    
    public List<RiasecCategory> PrimaryInterests => Scores
        .OrderByDescending(s => s.Value)
        .Take(3)
        .Select(s => s.Key)
        .ToList();
}