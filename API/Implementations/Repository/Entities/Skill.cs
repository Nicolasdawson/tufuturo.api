namespace API.Implementations.Repository.Entities;

public class Skill : GenericEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; } // "Soft" or "Hard"
}
