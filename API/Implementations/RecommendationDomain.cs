
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
        private readonly IQuestionsDomain _questionsDomain;
        private readonly ICatalogsDomain _catalogsDomain;

        public RecommendationDomain(
            IRepository<Entities.CareerCampus> careerCampusRepository,
            IQuestionsDomain questionsDomain,
            ICatalogsDomain catalogsDomain)
        {
            _careerCampusRepository = careerCampusRepository;
            _questionsDomain = questionsDomain;
            _catalogsDomain = catalogsDomain;
        }

        public async Task<Result<List<RecommendedCareer>>> GetRecommendations(AssessmentRequest request)
        {
            var questions = await  _questionsDomain.GetAllQuestions();
            
            var riasecScores = GetRiasecScores(request.AnswersRiasec, questions);

            var topCategory = riasecScores.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;

            if (string.IsNullOrEmpty(topCategory))
            {
                return Result<List<RecommendedCareer>>.Success(new List<RecommendedCareer>());
            }

            var knowledgeAreaNames = GetKnowledgeAreasForRiasec(topCategory);
            
            var allKnowledgeAreas = await _catalogsDomain.GetKnowledgeAreas();
            var knowledgeAreaIds = allKnowledgeAreas
                                    .Where(ka => knowledgeAreaNames.Contains(ka.Name))
                                    .Select(ka => ka.Id)
                                    .ToList();

            if (!knowledgeAreaIds.Any())
            {
                return Result<List<RecommendedCareer>>.Success(new List<RecommendedCareer>());
            }

            var careerCampuses = await _careerCampusRepository.Get(x => !x.IsDeleted 
                                                                        && knowledgeAreaIds.Contains(x.CareerInstitution.Career.KnowledgeAreaId))
                .Include(cc => cc.CareerInstitution)
                    .ThenInclude(ci => ci.Career) 
                    .ThenInclude(x => x.KnowledgeArea)
                .Include(cc => cc.CareerInstitution)
                    .ThenInclude(ci => ci.Institution)
                .Include(cc => cc.CareerInstitution)
                .Include(cc => cc.InstitutionCampus)
                .ToListAsync();

            var recommendations = careerCampuses
                .Select(cc => new RecommendedCareer
                {
                    CareerName = cc.CareerInstitution.Career.Name,
                    InstitutionName = cc.CareerInstitution.Institution.Name,
                    CampusName = cc.InstitutionCampus.Name,
                    MatchScore = CalculateMatchScore(cc, riasecScores) 
                })
                .OrderByDescending(r => r.MatchScore)
                .Take(10)
                .ToList();

            return Result<List<RecommendedCareer>>.Success(recommendations);
        }

        private Dictionary<string, int> GetRiasecScores(List<AnswerRequest> answers, List<Question> questions)
        {
            var scores = new Dictionary<string, int>();
            foreach (var category in Enum.GetNames(typeof(RiasecCategory)))
            {
                scores[category] = 0;
            }

            foreach (var answer in answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if(question is null)
                    continue;
                
                if (scores.ContainsKey(Enum.GetName(question.Category)))
                {
                    scores[Enum.GetName(question.Category)] += answer.Score;
                }
            }
            return scores;
        }

        private List<string> GetKnowledgeAreasForRiasec(string category)
        {
            return category switch
            {
                "Realistic" => new List<string> { "Tecnología", "Ciencias Básicas", "Agropecuaria" },
                "Investigative" => new List<string> { "Ciencias Básicas", "Salud", "Ciencias Sociales" },
                "Artistic" => new List<string> { "Arte y Arquitectura", "Humanidades" },
                "Social" => new List<string> { "Educación", "Salud", "Ciencias Sociales" },
                "Enterprising" => new List<string> { "Administración y Comercio", "Derecho" },
                "Conventional" => new List<string> { "Administración y Comercio", "Derecho" },
                _ => new List<string>(),
            };
        }
        
        private double CalculateMatchScore(Entities.CareerCampus careerCampus, Dictionary<string, int> riasecScores)
        {
            var knowledgeArea = careerCampus.CareerInstitution.Career.KnowledgeArea.Name;
            var primaryRiasecCategory = GetRiasecForKnowledgeArea(knowledgeArea);
            
            double interestScore = riasecScores.ContainsKey(primaryRiasecCategory) ? riasecScores[primaryRiasecCategory] / (double)(riasecScores.Values.Max() > 0 ? riasecScores.Values.Max() : 1) : 0;
            
            return interestScore;
        }

        private string GetRiasecForKnowledgeArea(string knowledgeArea)
        {
            return knowledgeArea switch
            {
                "Tecnología" or "Ciencias Básicas" or "Agropecuaria" => "Realistic",
                "Ciencias Básicas" or "Salud" or "Ciencias Sociales" => "Investigative",
                "Arte y Arquitectura" or "Humanidades" => "Artistic",
                "Educación" or "Salud" or "Ciencias Sociales" => "Social",
                "Administración y Comercio" or "Derecho" => "Enterprising",
                // "" => "Conventional",
                _ => string.Empty,
            };
        }
    }
}
