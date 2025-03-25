using API.Abstractions;
using API.Models;

namespace API.Implementations;

public class AssessmentService : IAssessmentService
{
    public async Task<List<Question>> GetQuestions()
    {
        return new List<Question>
        {
            // Realistic questions
            new Question
                { Id = 1, Text = "I enjoy working with tools and machines", Category = RiasecCategory.Realistic },
            new Question { Id = 2, Text = "I like to build things with my hands", Category = RiasecCategory.Realistic },
            new Question
            {
                Id = 3, Text = "I prefer activities that involve physical effort", Category = RiasecCategory.Realistic
            },
            new Question { Id = 4, Text = "I enjoy repairing mechanical devices", Category = RiasecCategory.Realistic },
            new Question { Id = 5, Text = "I am good at using tools", Category = RiasecCategory.Realistic },

            // Investigative questions
            new Question { Id = 6, Text = "I enjoy solving complex problems", Category = RiasecCategory.Investigative },
            new Question
                { Id = 7, Text = "I like to analyze data and information", Category = RiasecCategory.Investigative },
            new Question
            {
                Id = 8, Text = "I enjoy science and scientific experiments", Category = RiasecCategory.Investigative
            },
            new Question
                { Id = 9, Text = "I am curious about how things work", Category = RiasecCategory.Investigative },
            new Question
                { Id = 10, Text = "I enjoy challenging intellectual puzzles", Category = RiasecCategory.Investigative },

            // Artistic questions
            new Question { Id = 11, Text = "I enjoy expressing myself creatively", Category = RiasecCategory.Artistic },
            new Question
            {
                Id = 12, Text = "I like activities that allow for self-expression", Category = RiasecCategory.Artistic
            },
            new Question
                { Id = 13, Text = "I enjoy art, music, or creative writing", Category = RiasecCategory.Artistic },
            new Question
            {
                Id = 14, Text = "I prefer tasks without rigid rules or structures", Category = RiasecCategory.Artistic
            },
            new Question { Id = 15, Text = "I am good at thinking of new ideas", Category = RiasecCategory.Artistic },

            // Social questions
            new Question
                { Id = 16, Text = "I enjoy helping others with their problems", Category = RiasecCategory.Social },
            new Question { Id = 17, Text = "I like teaching or training others", Category = RiasecCategory.Social },
            new Question
                { Id = 18, Text = "I am good at understanding people's feelings", Category = RiasecCategory.Social },
            new Question { Id = 19, Text = "I enjoy working as part of a team", Category = RiasecCategory.Social },
            new Question
                { Id = 20, Text = "I am interested in social issues and causes", Category = RiasecCategory.Social },

            // Enterprising questions
            new Question
            {
                Id = 21, Text = "I enjoy persuading others to do things my way", Category = RiasecCategory.Enterprising
            },
            new Question { Id = 22, Text = "I like to lead and direct others", Category = RiasecCategory.Enterprising },
            new Question
            {
                Id = 23, Text = "I enjoy starting and carrying out projects", Category = RiasecCategory.Enterprising
            },
            new Question
            {
                Id = 24, Text = "I am good at selling things or promoting ideas", Category = RiasecCategory.Enterprising
            },
            new Question
                { Id = 25, Text = "I like taking risks to achieve goals", Category = RiasecCategory.Enterprising },

            // Conventional questions
            new Question
                { Id = 26, Text = "I enjoy working with numbers and data", Category = RiasecCategory.Conventional },
            new Question
            {
                Id = 27, Text = "I like following clear procedures and rules", Category = RiasecCategory.Conventional
            },
            new Question
            {
                Id = 28, Text = "I am good at organizing and keeping records", Category = RiasecCategory.Conventional
            },
            new Question { Id = 29, Text = "I pay attention to details", Category = RiasecCategory.Conventional },
            new Question
            {
                Id = 30, Text = "I prefer structured tasks with clear instructions",
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