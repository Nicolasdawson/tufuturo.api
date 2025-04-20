﻿using API.Abstractions;
using API.Models;
using Ardalis.Result;
using Entities = API.Implementations.Repository.Entities;

namespace API.Implementations;

public class AssessmentDomain : IAssessmentDomain
{
    private readonly IStudentRepository _studentRepository;
    private readonly IQuestionRepository _questionRepository;

    public AssessmentDomain(IStudentRepository studentRepository, IQuestionRepository questionRepository)
    {
        _studentRepository = studentRepository;
        _questionRepository = questionRepository;
    }
    
    public async Task<Result> CreateAssessment(AssessmentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId);
        
        if (student is null)
            return Result.NotFound("Student not found");

        var questions = await _questionRepository.GetAllQuestions();
        
        var scoreRiasec = GetRiasecScores(request.AnswersRiasec, questions);
        
        // filtrar por region opcional
        
        // filtrar por area
        
        // filtrar por notas
        
        // filtrar rango sueldo
        
        // ratio felicidad? ndeah si lo tengo
        
        // arancel opcional
        
        // guardar request
        
        // guardar respuesta al usuario
        
        // mandar mensaje a la cola para mandar el correo
        
        // ordenar las carrearas por mayor sueldo e empleabilidad (ns cuantas devolver)
        
        
        return Result.Success();
    }

    private Dictionary<string, int> GetRiasecScores(List<AnswerRequest> answers, List<Entities.Question> questions)
    {
        var scores = new Dictionary<string, int>();
        
        foreach (var category in Enum.GetValues<RiasecCategory>())
        {
            scores[nameof(category)] = 0;
        }
        
        foreach (var answer in answers)
        {
            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question != null)
            {
                scores[question.Category] += answer.Score;
            }
        }

        return scores;
    }
}