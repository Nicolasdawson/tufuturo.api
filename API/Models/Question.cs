﻿namespace API.Models;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public RiasecCategory Category { get; set; }
}