namespace API.Models;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; } // "Soft" or "Hard"
}
