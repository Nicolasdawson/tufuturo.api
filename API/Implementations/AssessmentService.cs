using API.Abstractions;
using API.Models;

namespace API.Implementations;

public class AssessmentService : IAssessmentService
{
    public async Task<List<Question>> GetQuestions()
    {
        return new List<Question>
        {
            new Question
                { Id = 1, Text = "Disfruto trabajar con herramientas y máquinas", Category = RiasecCategory.Realistic },
            new Question
                { Id = 2, Text = "Me gusta construir cosas con mis manos", Category = RiasecCategory.Realistic },
            new Question
            {
                Id = 3, Text = "Prefiero actividades que impliquen esfuerzo físico", Category = RiasecCategory.Realistic
            },
            new Question
                { Id = 4, Text = "Disfruto reparar dispositivos mecánicos", Category = RiasecCategory.Realistic },
            new Question { Id = 5, Text = "Soy bueno usando herramientas", Category = RiasecCategory.Realistic },
            new Question
                { Id = 6, Text = "Disfruto resolver problemas complejos", Category = RiasecCategory.Investigative },
            new Question
                { Id = 7, Text = "Me gusta analizar datos e información", Category = RiasecCategory.Investigative },
            new Question
            {
                Id = 8, Text = "Disfruto la ciencia y los experimentos científicos",
                Category = RiasecCategory.Investigative
            },
            new Question
            {
                Id = 9, Text = "Siento curiosidad por cómo funcionan las cosas", Category = RiasecCategory.Investigative
            },
            new Question
            {
                Id = 10, Text = "Disfruto los acertijos intelectuales desafiantes",
                Category = RiasecCategory.Investigative
            },
            new Question
                { Id = 11, Text = "Disfruto expresarme de forma creativa", Category = RiasecCategory.Artistic },
            new Question
            {
                Id = 12, Text = "Me gustan las actividades que permiten la autoexpresión",
                Category = RiasecCategory.Artistic
            },
            new Question
            {
                Id = 13, Text = "Disfruto el arte, la música o la escritura creativa",
                Category = RiasecCategory.Artistic
            },
            new Question
            {
                Id = 14, Text = "Prefiero tareas sin reglas ni estructuras rígidas", Category = RiasecCategory.Artistic
            },
            new Question { Id = 15, Text = "Soy bueno pensando en nuevas ideas", Category = RiasecCategory.Artistic },
            new Question
                { Id = 16, Text = "Disfruto ayudar a otros con sus problemas", Category = RiasecCategory.Social },
            new Question { Id = 17, Text = "Me gusta enseñar o capacitar a otros", Category = RiasecCategory.Social },
            new Question
            {
                Id = 18, Text = "Soy bueno entendiendo los sentimientos de las personas",
                Category = RiasecCategory.Social
            },
            new Question { Id = 19, Text = "Disfruto trabajar en equipo", Category = RiasecCategory.Social },
            new Question
                { Id = 20, Text = "Me interesan los temas y causas sociales", Category = RiasecCategory.Social },
            new Question
            {
                Id = 21, Text = "Disfruto persuadir a otros para que hagan las cosas a mi manera",
                Category = RiasecCategory.Enterprising
            },
            new Question
                { Id = 22, Text = "Me gusta liderar y dirigir a otros", Category = RiasecCategory.Enterprising },
            new Question
            {
                Id = 23, Text = "Disfruto iniciar y llevar a cabo proyectos", Category = RiasecCategory.Enterprising
            },
            new Question
            {
                Id = 24, Text = "Soy bueno vendiendo cosas o promoviendo ideas", Category = RiasecCategory.Enterprising
            },
            new Question
            {
                Id = 25, Text = "Me gusta asumir riesgos para lograr objetivos", Category = RiasecCategory.Enterprising
            },
            new Question
                { Id = 26, Text = "Disfruto trabajar con números y datos", Category = RiasecCategory.Conventional },
            new Question
            {
                Id = 27, Text = "Me gusta seguir procedimientos y reglas claras", Category = RiasecCategory.Conventional
            },
            new Question
            {
                Id = 28, Text = "Soy bueno organizando y manteniendo registros", Category = RiasecCategory.Conventional
            },
            new Question { Id = 29, Text = "Presto atención a los detalles", Category = RiasecCategory.Conventional },
            new Question
            {
                Id = 30, Text = "Prefiero tareas estructuradas con instrucciones claras",
                Category = RiasecCategory.Conventional
            }
        };
    }

    public async Task<AssessmentResult> CalculateResult(Guid userId, List<Answer> answers)
    {
        var questions = await GetQuestions();

        var scores = new Dictionary<RiasecCategory, int>();
        foreach (var category in Enum.GetValues<RiasecCategory>())
        {
            scores[category] = 0;
        }

        foreach (var answer in answers)
        {
            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question != null)
            {
                scores[question.Category] += answer.Score;
            }
        }

        var result = new AssessmentResult
        {
            UserId = userId,
            CompletedAt = DateTime.UtcNow,
            Scores = scores
        };

        return result;
    }

    public async Task<List<CareerSuggestion>> GetCareerSuggestions(AssessmentResult result)
    {
        var primaryInterests = result.PrimaryInterests;
        return await GetCareersByInterests(primaryInterests);
    }

    private async Task<List<CareerSuggestion>> GetCareersByInterests(List<RiasecCategory> interests)
    {
        var careers = new List<Career>
        {
            // Realistic careers
            new Career
            {
                Id = 1,
                Title = "Mechanical Engineer",
                Description = "Designs, develops, and tests mechanical devices and systems.",
                Categories = "Realistic,Investigative",
                EducationRequirements = "Bachelor's degree in Mechanical Engineering",
                MedianSalary = 88000
            },
            new Career
            {
                Id = 2,
                Title = "Electrician",
                Description = "Installs, maintains, and repairs electrical systems.",
                Categories = "Realistic",
                EducationRequirements = "Vocational training or apprenticeship",
                MedianSalary = 56000
            },

            // Investigative careers
            new Career
            {
                Id = 3,
                Title = "Data Scientist",
                Description = "Analyzes complex data to help guide business decisions.",
                Categories = "Investigative,Conventional",
                EducationRequirements = "Master's or PhD in Computer Science, Statistics, or related field",
                MedianSalary = 122000
            },
            new Career
            {
                Id = 4,
                Title = "Biologist",
                Description = "Studies living organisms and their relationship to the environment.",
                Categories = "Investigative,Realistic",
                EducationRequirements = "Bachelor's degree in Biology; advanced research requires PhD",
                MedianSalary = 65000
            },

            // Artistic careers
            new Career
            {
                Id = 5,
                Title = "Graphic Designer",
                Description = "Creates visual concepts to communicate ideas.",
                Categories = "Artistic,Enterprising",
                EducationRequirements = "Bachelor's degree in Graphic Design or related field",
                MedianSalary = 53000
            },
            new Career
            {
                Id = 6,
                Title = "Music Composer",
                Description = "Creates and arranges original music.",
                Categories = "Artistic",
                EducationRequirements = "Formal education not always required; Bachelor's in Music composition helpful",
                MedianSalary = 51000
            },

            // Social careers
            new Career
            {
                Id = 7,
                Title = "School Counselor",
                Description = "Helps students develop academic and social skills.",
                Categories = "Social,Enterprising",
                EducationRequirements = "Master's degree in School Counseling or related field",
                MedianSalary = 57000
            },
            new Career
            {
                Id = 8,
                Title = "Registered Nurse",
                Description = "Provides patient care and education.",
                Categories = "Social,Investigative",
                EducationRequirements = "Associate's or Bachelor's degree in Nursing",
                MedianSalary = 75000
            },

            // Enterprising careers
            new Career
            {
                Id = 9,
                Title = "Marketing Manager",
                Description = "Plans and directs marketing programs and strategies.",
                Categories = "Enterprising,Artistic",
                EducationRequirements = "Bachelor's degree in Marketing or Business",
                MedianSalary = 135000
            },
            new Career
            {
                Id = 10,
                Title = "Sales Representative",
                Description = "Sells products or services to businesses or consumers.",
                Categories = "Enterprising,Social",
                EducationRequirements = "High school diploma; Bachelor's degree preferred for technical sales",
                MedianSalary = 62000
            },

            // Conventional careers
            new Career
            {
                Id = 11,
                Title = "Accountant",
                Description = "Prepares and examines financial records.",
                Categories = "Conventional,Enterprising",
                EducationRequirements = "Bachelor's degree in Accounting or related field",
                MedianSalary = 71000
            },
            new Career
            {
                Id = 12,
                Title = "Database Administrator",
                Description = "Manages and organizes data, including financial information.",
                Categories = "Conventional,Investigative",
                EducationRequirements = "Bachelor's degree in Computer Science or related field",
                MedianSalary = 93000
            }
        };

        var result = new List<CareerSuggestion>();
        foreach (var career in careers)
        {
            var careerCategories = career.Categories
                .Split(',')
                .Select(c => Enum.Parse<RiasecCategory>(c))
                .ToList();

            if (interests.Any(i => careerCategories.Contains(i)))
            {
                result.Add(new CareerSuggestion
                {
                    Title = career.Title,
                    Description = career.Description,
                    RelatedCategories = careerCategories,
                    EducationRequirements = career.EducationRequirements,
                    MedianSalary = career.MedianSalary
                });
            }
        }

        return result;
    }
}