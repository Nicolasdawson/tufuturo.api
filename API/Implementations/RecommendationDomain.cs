
using API.Abstractions;
using API.Models;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Entities = API.Implementations.Repository.Entities;

namespace API.Implementations
{
    public class RecommendationDomain : IRecommendationDomain
    {
        private readonly IRepository<Entities.CareerCampus> _careerCampusRepository;
        private readonly IRepository<Entities.Question> _questionRepository;
        private readonly IRepository<Entities.KnowledgeArea> _knowledgeAreaRepository;

        public RecommendationDomain(
            IRepository<Entities.CareerCampus> careerCampusRepository,
            IRepository<Entities.Question> questionRepository,
            IRepository<Entities.KnowledgeArea> knowledgeAreaRepository)
        {
            _careerCampusRepository = careerCampusRepository;
            _questionRepository = questionRepository;
            _knowledgeAreaRepository = knowledgeAreaRepository;
        }

        public async Task<Result<List<RecommendedCareer>>> GetRecommendations(AssessmentRequest request)
        {
            var questions = await _questionRepository.GetAllAsync();
            var riasecScores = GetRiasecScores(request.AnswersRiasec, questions.ToList());

            var topCategory = riasecScores.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;

            if (string.IsNullOrEmpty(topCategory))
            {
                return Result<List<RecommendedCareer>>.Success(new List<RecommendedCareer>());
            }

            var knowledgeAreaNames = GetKnowledgeAreasForRiasec(topCategory);
            var allKnowledgeAreas = await _knowledgeAreaRepository.GetAllAsync();
            var knowledgeAreaIds = allKnowledgeAreas
                                    .Where(ka => knowledgeAreaNames.Contains(ka.Name))
                                    .Select(ka => ka.Id)
                                    .ToList();

            if (!knowledgeAreaIds.Any())
            {
                return Result<List<RecommendedCareer>>.Success(new List<RecommendedCareer>());
            }

            var careerCampuses = await _careerCampusRepository.Get()
                .Include(cc => cc.CareerInstitution)
                    .ThenInclude(ci => ci.Career)
                .Include(cc => cc.CareerInstitution)
                    .ThenInclude(ci => ci.Institution)
                .Include(cc => cc.CareerInstitution)
                    .ThenInclude(ci => ci.CareerInstitutionStats)
                .Include(cc => cc.InstitutionCampus)
                .Where(cc => knowledgeAreaIds.Contains(cc.CareerInstitution.Career.KnowledgeAreaId))
                .ToListAsync();

            var recommendations = careerCampuses
                .Select(cc => new RecommendedCareer
                {
                    CareerName = cc.CareerInstitution.Career.Name,
                    InstitutionName = cc.CareerInstitution.Institution.Name,
                    CampusName = cc.InstitutionCampus.Name,
                    EmployabilityFirstYear = cc.CareerInstitution.CareerInstitutionStats.FirstOrDefault()?.EmployabilityFirstYear ?? 0,
                    AverageSalaryFrom = cc.CareerInstitution.CareerInstitutionStats.FirstOrDefault()?.AvarageSalaryFrom ?? 0,
                    MatchScore = CalculateMatchScore(cc, riasecScores) 
                })
                .OrderByDescending(r => r.MatchScore)
                .Take(10)
                .ToList();

            return Result<List<RecommendedCareer>>.Success(recommendations);
        }

        private Dictionary<string, int> GetRiasecScores(List<AnswerRequest> answers, List<Entities.Question> questions)
        {
            var scores = new Dictionary<string, int>();
            foreach (var category in Enum.GetNames(typeof(RiasecCategory)))
            {
                scores[category] = 0;
            }

            foreach (var answer in answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question != null && scores.ContainsKey(question.Category))
                {
                    scores[question.Category] += answer.Score;
                }
            }
            return scores;
        }

        private List<string> GetKnowledgeAreasForRiasec(string category)
        {
            return category switch
            {
                "Realistic" => new List<string> { "Technology", "Natural and Exact Sciences", "Agricultural Sciences" },
                "Investigative" => new List<string> { "Natural and Exact Sciences", "Health Sciences", "Social Sciences" },
                "Artistic" => new List<string> { "Arts and Architecture", "Humanities" },
                "Social" => new List<string> { "Education", "Health Sciences", "Social Sciences" },
                "Enterprising" => new List<string> { "Administration and Commerce", "Law" },
                "Conventional" => new List<string> { "Administration and Commerce", "Law" },
                _ => new List<string>(),
            };
        }
        
        private double CalculateMatchScore(Entities.CareerCampus careerCampus, Dictionary<string, int> riasecScores)
        {
            // Normalize employability and salary to a 0-1 scale
            // These max values are assumptions and should be adjusted based on real data ranges
            const decimal maxEmployability = 100.0m; 
            const int maxSalary = 4000000; 

            var stats = careerCampus.CareerInstitution.CareerInstitutionStats.FirstOrDefault();
            if (stats == null) return 0;

            double employabilityScore = (double)(stats.EmployabilityFirstYear / maxEmployability);
            double salaryScore = (double)stats.AvarageSalaryFrom / maxSalary;

            // Get the career's knowledge area and map it back to a primary RIASEC category
            var knowledgeArea = careerCampus.CareerInstitution.Career.KnowledgeArea.Name;
            var primaryRiasecCategory = GetRiasecForKnowledgeArea(knowledgeArea);
            
            // Get the student's score for that category, normalized
            double interestScore = riasecScores.ContainsKey(primaryRiasecCategory) ? riasecScores[primaryRiasecCategory] / (double)(riasecScores.Values.Max() > 0 ? riasecScores.Values.Max() : 1) : 0;

            // Combine scores with weighting
            // Example weighting: 50% interest, 30% employability, 20% salary
            return (interestScore * 0.5) + (employabilityScore * 0.3) + (salaryScore * 0.2);
        }

        private string GetRiasecForKnowledgeArea(string knowledgeArea)
        {
            // This is the reverse mapping of GetKnowledgeAreasForRiasec
            // It's simplified; a knowledge area can belong to multiple RIASEC categories.
            // We are picking a "primary" one for this calculation.
            return knowledgeArea switch
            {
                "Technology" => "Realistic",
                "Natural and Exact Sciences" => "Investigative",
                "Agricultural Sciences" => "Realistic",
                "Health Sciences" => "Social",
                "Social Sciences" => "Social",
                "Arts and Architecture" => "Artistic",
                "Humanities" => "Artistic",
                "Administration and Commerce" => "Enterprising",
                "Law" => "Conventional",
                _ => string.Empty,
            };
        }
    }
}
