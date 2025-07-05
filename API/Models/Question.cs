namespace API.Models;

public class Question
{
    public required int Id { get; set; }
    public required string Text { get; set; }
    public required RiasecCategory Category { get; set; }
}