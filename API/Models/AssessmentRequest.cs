﻿using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class AssessmentRequest
{
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public List<AnswerRequest> AnswersRiasec { get; set; }
    
    [Required]
    public int RankingScore { get; set; }
    
    [Required]
    public int NemScore { get; set; }
    
    [Required]
    public int ReadingScore { get; set; }
    
    [Required]
    public int MathsScore { get; set; }
    
    public int? SciencesScore { get; set; }
    
    public int? MathsOptionalScore { get; set; }
    
    public int? HistoryScore { get; set; }
    
}